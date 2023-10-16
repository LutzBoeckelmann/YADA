// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback.Recorder.Recordings
{
    internal interface IRecording : IFeedbackVisitor, IDisposable
    {
        void Closed(IRecording current);
        IDependencyContextVisitor<string> ContextVisitor { get; }

        bool ReadMode { get; }
    }
}