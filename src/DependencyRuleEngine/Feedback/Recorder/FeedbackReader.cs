// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.IO;
using YADA.DependencyRuleEngine.Feedback.Recorder.Recordings;

namespace YADA.DependencyRuleEngine.Feedback.Recorder
{
    public class FeedbackReader
    {
        private readonly FeedbackRoot m_Parent = new FeedbackRoot(true);
        
        public void ReadRecording(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                var currentLine = line.TrimStart();
                
                if (currentLine.StartsWith("Type: "))
                {
                    m_Parent.Type(currentLine.Substring("Type: ".Length));
                }
                else if (currentLine.StartsWith("Rule: "))
                {
                    m_Parent.ViolatedRule(currentLine.Substring("Rule: ".Length));
                }
                else if (currentLine.StartsWith("Dependency: "))
                {
                    m_Parent.ForbiddenDependency(currentLine.Substring("Dependency: ".Length));
                }
                else if (currentLine.StartsWith("Context: "))
                {
                    m_Parent.Context(new RecordedContext(currentLine.Substring("Context: ".Length)));
                   
                }
                else if (currentLine.StartsWith("Info: "))
                {
                    m_Parent.Info(currentLine.Substring("Info: ".Length));

                }
            }
        }
      

        internal List<TypeRecording> Types ()
        {
            return m_Parent.Types();
        }

        public IList<string> GetResult() 
        {   
            return FeedbackPrinter.Print(Types());
        }
    }
}