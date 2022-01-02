using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public class FeedbackReader 
    {
        private readonly Stack<object> m_CurrentContext = new Stack<object>();

        public void ReadRecording(string file) 
        {
            var lines = File.ReadAllLines(file);
            InternalDo(lines);
        }

        internal List<TypeRecording> Types ()
        {
            var types = m_CurrentContext.Cast<TypeRecording>().ToList();
            types.Reverse();
            return types;
        }

        private void InternalDo(IEnumerable<string> lines)
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
        public IList<string> GetResult() 
        {   
            return FeedbackPrinter.Print(Types());
        }
    }
}