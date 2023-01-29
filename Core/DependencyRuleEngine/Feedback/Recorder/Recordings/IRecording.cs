// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal interface IRecording : IFeedbackVisitor, IDisposable
    {
        void Closed(IRecording current);
        IDependencyContextVisitor<string> ContextVisitor { get; }

        bool ReadMode { get; }
    }
}