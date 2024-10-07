// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;

namespace YADA.ArchGuard.Feedback
{
    public interface IFeedbackCollector
    {
        List<Tuple<FeedbackType, string>> CollectedFeedback { get; }
    }
}

