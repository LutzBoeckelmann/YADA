// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
  
    

    internal class DependencyFeedback : IDependencyFeedback
    {
        private readonly List<string> m_Contexted = new List<string>();
        public IDependencyFeedback At(string context)
        {
            m_Contexted.Add(context);
            return this;
        }

        internal void Print(StringBuilder result)
        {
            foreach (var context in m_Contexted)
            {
                
                result.AppendLine($"    At: {context}");
            }
        }
    }
}

