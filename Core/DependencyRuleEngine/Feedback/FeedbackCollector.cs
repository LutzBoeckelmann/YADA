// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;

namespace YADA.Core.DependencyRuleEngine.Feedback
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

        public IEnumerable<Tuple<string, string>> GetFeedback() 
        {
            foreach(var pair in m_Feedbacks) 
            {
                StringBuilder result = new StringBuilder();
                pair.Value.Print(result);
                yield return new Tuple<string, string>(pair.Key, result.ToString());
            }
        }

        public StringBuilder Print()
        {
            StringBuilder result = new StringBuilder();

            foreach (var pair in m_Feedbacks)
            {
                result.AppendLine(pair.Key);
                pair.Value.Print(result);
            }

            return result;
        }
    }
}

