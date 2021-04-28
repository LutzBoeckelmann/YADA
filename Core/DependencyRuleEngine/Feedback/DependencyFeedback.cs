// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;

namespace YADA.Core.DependencyRuleEngine.Impl
{
    public class DependencyFeedback : IDependencyFeedback
    {
        private readonly List< string> m_Contexted = new List<string>();
        public IDependencyFeedback At(string context)
        {
            m_Contexted.Add(context);
            return this;
        }

        internal void Print(StringBuilder result)
        {
            foreach(var context in m_Contexted) 
            {
                result.AppendLine($"    At: {context}");
            }
        }
    }
}

