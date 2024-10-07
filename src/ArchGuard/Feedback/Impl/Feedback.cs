// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;

namespace YADA.ArchGuard.Feedback.Impl
{
    public class FeedbackCollector : ICheckFeedback, IFeedbackCollector
    {
        private List<Tuple<FeedbackType,string>> m_Feedback = new List<Tuple<FeedbackType, string>>();
        
        public void AddFeedback(FeedbackType feedbackType, string feedback)
        {
            m_Feedback.Add(Tuple.Create(feedbackType, feedback));
        }

        public void AddFeedback(string feedback)
        {
            AddFeedback(FeedbackType.Error, feedback);
        }

        public void Clean()
        {
            m_Feedback.Clear();
        }

        public List<Tuple<FeedbackType, string>> CollectedFeedback => m_Feedback;
    }
}

