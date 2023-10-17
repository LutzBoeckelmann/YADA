// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;

namespace YADA.DependencyRuleEngine.Feedback
{
    internal class RuleFeedback : IRuleFeedback
    {
        private readonly List<string> m_Infos = new List<string>();
        private readonly Dictionary<string, DependencyFeedback> m_ViolatedDependency = new Dictionary<string, DependencyFeedback>();

        public IRuleFeedback AddInfo(string info)
        {
            m_Infos.Add(info);
            return this;
        }

        public IDependencyFeedback ForbiddenDependency(string dependency)
        {
            if (!m_ViolatedDependency.ContainsKey(dependency))
            {
                m_ViolatedDependency.Add(dependency, new DependencyFeedback());
            }

            return m_ViolatedDependency[dependency];
        }

        public void Explore(IFeedbackVisitor visitor)
        {
            foreach (var msg in m_Infos)
            {
                visitor.Info(msg).Dispose();
            }

            foreach (var dependency in m_ViolatedDependency)
            {
                using (visitor.ForbiddenDependency(dependency.Key))
                {
                    dependency.Value.Explore(visitor);
                }
            }
        }
    }
}

