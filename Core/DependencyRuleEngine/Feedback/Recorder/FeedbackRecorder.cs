using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{

    // new approach
    public class FeedbackRecorderNew : IFeedbackVisitor
    {
        public IDisposable Context(IDependencyContext context)
        {
            throw new NotImplementedException();
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            throw new NotImplementedException();
        }

        public IDisposable Info(string msg)
        {
            throw new NotImplementedException();
        }

        public IDisposable Type(string type)
        {
            throw new NotImplementedException();
        }

        public IDisposable ViolatedRule(string rule)
        {
            throw new NotImplementedException();
        }
    }

    //

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

        private readonly Stack<object> m_CurrentContext = new Stack<object>();
      
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
            
            return FeedbackPrinter.Print(types);
        }

        public void WriteFeedbackResults(string file)
        {
            var types = m_CurrentContext.Cast<TypeRecording>().ToList();
            types.Reverse();


            File.WriteAllLines(file, FeedbackPrinter.Print(types));
        }
    }
}