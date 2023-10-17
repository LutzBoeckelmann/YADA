// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback
{
    internal class DependencyFeedback : IDependencyFeedback
    {
        private readonly List<IDependencyContext> m_Contexted = new List<IDependencyContext>();
        public IDependencyFeedback At(IDependencyContext context)
        {
            m_Contexted.Add(context);
            return this;
        }

        public void Explore(IFeedbackVisitor visitor)
        {
            foreach (var context in m_Contexted)
            {
                using (visitor.Context(context)) { ; }
            }
        }
    }
}

