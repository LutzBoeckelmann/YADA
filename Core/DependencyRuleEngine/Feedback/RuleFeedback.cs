// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;

namespace YADA.Core.DependencyRuleEngine.Impl
{
    public class RuleFeedback : IRuleFeedback
    {
        private readonly List<string> m_Infos = new List<string>();
        private readonly Dictionary<string, DependencyFeedback> m_ViolatedDependency = new Dictionary<string, DependencyFeedback>();

        public IRuleFeedback AddInfo(string name)
        {
            m_Infos.Add(name);
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

        internal void Print(StringBuilder result)
        {
            foreach(var msg in m_Infos) 
            {
                result.AppendLine($"    Info: {msg}");
            }

            foreach(var dependency in m_ViolatedDependency) 
            {
                result.AppendLine($"    ForbiddenDependency {dependency.Key}");
                dependency.Value.Print(result);
            }
        }
    }
}

