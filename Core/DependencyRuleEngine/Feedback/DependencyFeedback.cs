// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

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

        public void Explore(IFeedbackVisitor visitor)
        {
            foreach (var context in m_Contexted)
            {
                visitor.Context(context);
            }
        }
    }
}

