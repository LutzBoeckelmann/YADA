// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.ArchGuard.Feedback
{
    public interface ICheckFeedback
    {
        void AddFeedback(FeedbackType feedbackType, string feedback);
        void AddFeedback(string feedback);
    }
}

