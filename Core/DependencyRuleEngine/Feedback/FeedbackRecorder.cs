using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public class ContextRecorder : IDependencyContextVisitor<string>
    {
        public string BaseClassDefinition(string name)
        {
            return $"{nameof(BaseClassDefinition)}_{name}";
        }

        public string FieldDefinition(string fieldName)
        {
            return $"{nameof(FieldDefinition)}_{fieldName}";
        }

        public string MethodBodyAccessedFieldType(string methodName, string fieldName)
        {
            return $"{nameof(MethodBodyAccessedFieldType)}_{methodName}_{fieldName}";
        }

        public string MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName)
        {
            return $"{nameof(MethodBodyCalledMethodParameter)}_{methodName}_{calledMethodFullName}";
        }

        public string MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName)
        {
            return $"{nameof(MethodBodyCalledMethodReturnType)}_{methodName}_{calledMethodFullName}";
        }

        public string MethodBodyCallMethodAtType(string methodName, string calledMethodFullName)
        {
            return $"{nameof(MethodBodyCallMethodAtType)}_{methodName}_{calledMethodFullName}";
        }

        public string MethodBodyReferencedType(string methodName)
        {
            return $"{nameof(MethodBodyReferencedType)}_{methodName}";
        }

        public string MethodDefinitionLocalVariable(string methodName)
        {
            return $"{nameof(MethodDefinitionLocalVariable)}_{methodName}";
        }

        public string MethodDefinitionParameter(string methodName)
        {
            return $"{nameof(MethodDefinitionParameter)}_{methodName}";
        }

        public string MethodDefinitionReturnType(string methodName)
        {
            return $"{nameof(MethodDefinitionReturnType)}_{methodName}";
        }
    }

    public class FeedbackReader 
    {
        public FeedbackReader()
        {

        }
        
        private readonly Stack<object> m_CurrentContext = new Stack<object>();

        private T PopUntil<T>()
        {
            while (m_CurrentContext.Count > 0 && m_CurrentContext.Peek().GetType().FullName != typeof(T).FullName)
            {
                m_CurrentContext.Pop();
            }

            if (m_CurrentContext.Count > 0)
            {
                return (T)m_CurrentContext.Peek();
            }
            return default(T);
        }

        internal List<TypeRecording> Types ()
        {
            var types = m_CurrentContext.Cast<TypeRecording>().ToList();
            types.Reverse();
            return types;
        }

        public void Do(string file) 
        {
            var lines = File.ReadAllLines(file);
            InternalDo(lines);
        }

        public void InternalDo(IEnumerable<string> lines)
        {
            foreach(var line in lines) 
            {
                var currentLine = line.TrimStart();
                if(currentLine.StartsWith("Type: ")) 
                {
                    PopUntil<TypeRecording>();

                    var current = new TypeRecording { TypeName = currentLine.Substring("Type: ".Length) };

                    m_CurrentContext.Push(current);
                }
                else if (currentLine.StartsWith("Rule: "))
                {
                    var currentType = PopUntil<TypeRecording>();
                    var current = new RuleRecording() { RuleName = currentLine.Substring("Rule: ".Length) };
                    currentType.Add(current);
                    m_CurrentContext.Push(current);
                } 
                else if(currentLine.StartsWith("Dependency: "))
                {
                    var currentRule = PopUntil<RuleRecording>();
                    var current = new DependencyRecording() { DependencyName = currentLine.Substring("Dependency: ".Length) };
                    m_CurrentContext.Push(current);
                    currentRule.Add(current);
                }
                else if(currentLine.StartsWith("Context: ")) 
                {
                    var currentDependency = PopUntil<DependencyRecording>();
                    var current = new ContextRecording() { Context = currentLine.Substring("Context: ".Length) };
                    currentDependency.Add(current);
                }
                else if(currentLine.StartsWith("Info: ")) 
                {
                    var currentRule = PopUntil<RuleRecording>();
                    currentRule.AddInfo(new InfoRecording() {Info = currentLine.Substring("Info: ".Length)}) ;
                }
            }

            PopUntil<TypeRecording>();
        }

        public IList<string> GetResult() 
        {   
            return Printer.Print(Types());
        }
    }

    internal class Printer 
    {
        public static IList<string> Print(List<TypeRecording> types) 
        {
            IList<string> result = new List<string>();
            
            foreach(TypeRecording recordedType in types) 
            {
                result.Add($"Type: {recordedType.TypeName}");
            
                foreach(var rule in recordedType.Rules) 
                {
                    result.Add($"Rule: {rule.RuleName}");
                    
                    foreach(var dependency in rule.Dependencies) 
                    {
                        result.Add($"Dependency: {dependency.DependencyName}");
                        
                        foreach(var context in dependency.Contexts) 
                        {
                            result.Add($"Context: {context.Context}");
                        }
                    }

                    foreach(var info in rule.Infos) 
                    {
                        result.Add($"Info: {info.Info}");
                    }
                }
            }
            
            return result;
        }
    }

    public class FeedbackFilter : IFeedbackVisitor
    {   
        private class Callback : IDisposable
        {
            private readonly Action m_Callback;

            public Callback(Action callback)
            {
                m_Callback = callback;
            }

            public void Dispose()
            {
                m_Callback();
            }
        }
        private readonly List<TypeRecording> m_Types;

        private IFeedbackVisitor m_Next;
        private TypeRecording m_CurrentType;
        private DependencyRecording m_CurrentDependency;
        
        private RuleRecording m_CurrentRule;

        private ContextRecorder m_Context = new ContextRecorder();

        private void NotifyType() 
        {
            if(m_TypeNotNotified) 
            {
                m_NextTypeDisposable = m_Next.Type(m_CurrentType.TypeName);
            }
            m_TypeNotNotified = false;
        }
        private bool m_TypeNotNotified;

        private IDisposable m_NextTypeDisposable;

        private void NotifyRule() 
        {
            NotifyType();

            if(m_RuleNotNotified) 
            {
                m_NextRuleDisposable = m_Next.ViolatedRule(m_CurrentRule.RuleName);
            }
            m_RuleNotNotified = false;
        }
        private bool m_RuleNotNotified;
        private IDisposable m_NextRuleDisposable;
        private void NotifyDependency() 
        {
            NotifyRule();
            if(m_DependencyNotNotified) 
            {
                m_Next.ForbiddenDependency(m_CurrentDependency.DependencyName);
            }
        }

        private bool m_DependencyNotNotified;
        private IDisposable m_NextDependencyDisposable;
        
        public FeedbackFilter(string file, IFeedbackVisitor next) 
        {
            var reader = new FeedbackReader();
            reader.Do(file);
            m_Types = reader.Types();

            m_Next = next;
        }

        internal FeedbackFilter(List<TypeRecording> types, IFeedbackVisitor next)
        {

            m_Next = next;
        }

        public IDisposable Context(IDependencyContext context)
        {
            IDisposable nextContextDisposable = null;
            ContextRecording currentContext = null;
            
            if (m_CurrentDependency != null)
            {
                currentContext = m_CurrentDependency.Contexts.FirstOrDefault(t => t.Context == context.Visit(m_Context));
            }

            if(currentContext == null)
            {
                NotifyDependency();
                nextContextDisposable = m_Next.Context(context);
            }

            return new Callback(() => { nextContextDisposable?.Dispose(); });
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            if(m_CurrentRule != null) 
            {
                m_CurrentDependency = m_CurrentRule.Dependencies.FirstOrDefault(t => t.DependencyName == dependency);
            }

            if(m_CurrentDependency != null) 
            {
                m_DependencyNotNotified = true;
            }
            else 
            {
                NotifyRule();
                m_NextDependencyDisposable = m_Next.ForbiddenDependency(dependency);
            }

            return new Callback(() => { m_DependencyNotNotified = false; m_CurrentDependency = null; m_NextDependencyDisposable?.Dispose(); });
        }

        public IDisposable Info(string msg)
        {
            InfoRecording currentInfo = null;
            if(m_CurrentRule != null) 
            {
                currentInfo = m_CurrentRule.Infos.FirstOrDefault(t => t.Info == msg);
            }
            IDisposable disposable = null;
            if(currentInfo == null) 
            {
                NotifyRule();
                disposable = m_Next.Info(msg);
            }
           
            return new Callback(() => { disposable?.Dispose(); });
        }

     
        public IDisposable Type(string type)
        {
            m_CurrentType = m_Types.FirstOrDefault(t => t.TypeName == type);
            
            if(m_CurrentType == null) 
            {
                m_NextTypeDisposable = m_Next.Type(type);
            }
            else 
            {
                m_TypeNotNotified = true;
            }

            return new Callback(
                () => { m_CurrentType = null; m_TypeNotNotified = false; m_NextTypeDisposable?.Dispose(); });
        }

        public IDisposable ViolatedRule(string rule)
        {
            if(m_CurrentType != null) 
            {
                m_CurrentRule = m_CurrentType.Rules.FirstOrDefault(t => t.RuleName == rule);
            }
            
            if(m_CurrentRule == null) 
            {
                NotifyType();
                m_NextRuleDisposable = m_Next.ViolatedRule(rule);
            }
            else
            {
                m_RuleNotNotified = true;
            }

            return new Callback(
                () => { m_CurrentRule = null; m_RuleNotNotified = false; m_NextRuleDisposable?.Dispose(); });
        }
    }

   
    internal class TypeRecording
    {
        public string TypeName { get; set; }

        public List<RuleRecording> Rules { get; set; } = new List<RuleRecording>();

        public void Add(RuleRecording item)
        {
            Rules.Add(item);
        }
    }

    internal class RuleRecording
    {
        public string RuleName { get; set; }

        public List<DependencyRecording> Dependencies { get; set; } = new List<DependencyRecording>();
        public List<InfoRecording> Infos { get; set; } = new List<InfoRecording>();

        public void Add(DependencyRecording item)
        {
            Dependencies.Add(item);
        }

        internal void AddInfo(InfoRecording infoRecording)
        {
            Infos.Add(infoRecording);
        }
    }

    internal class DependencyRecording
    {
        public string DependencyName { get; set; }

        public List<ContextRecording> Contexts { get; set; } = new List<ContextRecording>();

        public void Add(ContextRecording item)
        {
            Contexts.Add(item);
        }
    }

    internal class InfoRecording 
    {
        public string Info { get; set; }
    }

    internal class ContextRecording 
    {
        public string Context { get; set; }
    }
    
    public class FeedbackRecorder : IFeedbackVisitor
    {
        private class Callback : IDisposable
        {
            private readonly Action m_Callback;

            public Callback(Action callback)
            {
                m_Callback = callback;
            }

            public void Dispose()
            {
                m_Callback();
            }
        }

        private readonly IDependencyContextVisitor<string> m_DependencyContextVisitor = new ContextRecorder();

        private Stack<object> m_CurrentContext = new Stack<object>();
      
        public IDisposable Context(IDependencyContext context)
        {
            var currentDependency = (DependencyRecording)m_CurrentContext.Peek();
            currentDependency.Add(new ContextRecording() { Context = context.Visit(m_DependencyContextVisitor) });

            return new Callback(()=>{});
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            var rule = (RuleRecording)m_CurrentContext.Peek();
            var newContext = new DependencyRecording() { DependencyName = dependency };

            rule.Add(newContext);
            m_CurrentContext.Push(newContext);
            
            return new Callback(DisposeStacked);
        }

        public IDisposable Info(string msg)
        {
            var rule = (RuleRecording)m_CurrentContext.Peek();
            
            rule.AddInfo(new InfoRecording() { Info = msg });
            
            return new Callback(()=>{});
        }

        public IDisposable Type(string type)
        {
            var current = new TypeRecording { TypeName = type };
        
            m_CurrentContext.Push(current);
            
            return new Callback(()=>{});
        }
        
        private void DisposeStacked() 
        {
            m_CurrentContext.Pop();
        }

        public IDisposable ViolatedRule(string rule)
        {
            var currentContext = (TypeRecording)m_CurrentContext.Peek();
            var ruleContext = new RuleRecording() { RuleName = rule };
            currentContext.Add(ruleContext);
            m_CurrentContext.Push(ruleContext);

            return new Callback(DisposeStacked);
        }

        public IList<string> GetResult() 
        {
            var types = m_CurrentContext.Cast<TypeRecording>().ToList();
            types.Reverse();
            
            return Printer.Print(types);
        }

        public void WriteFeedbackResults(string file)
        {
            var types = m_CurrentContext.Cast<TypeRecording>().ToList();
            types.Reverse();


            File.WriteAllLines(file, Printer.Print(types));
        }
    }
}