// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;

namespace YADA.DependencyRuleEngine.Feedback
{
    public class FeedbackCollector : IFeedbackCollector
    {
        private readonly IDictionary<string, TypeFeedback> m_Feedbacks = new Dictionary<string, TypeFeedback>();
        public ITypeFeedback AddFeedbackForType(string type)
        {
            if (!m_Feedbacks.ContainsKey(type))
            {
                m_Feedbacks.Add(type, new TypeFeedback());
            }

            return m_Feedbacks[type];
        }

        public void Explore(IFeedbackVisitor collector) 
        {
            foreach(var pair in m_Feedbacks) 
            {
                using (collector.Type(pair.Key))
                {
                    pair.Value.Explore(collector);
                }
            }
        }
    }
}
