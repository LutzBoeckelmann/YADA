// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback.Recorder.Recordings
{
    internal class ContextRecording : IRecording
    {
        private readonly IRecording m_Parent;

        public ContextRecording(string context, IRecording parent) 
        {
            Context = context;
            m_Parent = parent;
        }
        public string Context { get; }

        public IDependencyContextVisitor<string> ContextVisitor => throw new NotImplementedException();

        public bool ReadMode => m_Parent.ReadMode;

        public void Closed(IRecording current)
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            //do not inform parent
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            throw new NotImplementedException();
        }

        public IDisposable Info(string msg)
        {
            throw new NotImplementedException();
        }

        public IDisposable Type(string type)
        {
            throw new NotImplementedException();
        }

        public IDisposable ViolatedRule(string rule)
        {
            throw new NotImplementedException();
        }

        IDisposable IFeedbackVisitor.Context(IDependencyContext context)
        {
            throw new NotImplementedException();
        }
    }

    internal class RecordedContext : IDependencyContext
    {
        public string SerializedContext { get; }

        public RecordedContext(string serializedContext)
        {
            SerializedContext = serializedContext;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor)
        {
            return (T)(object)SerializedContext;
        }
    }
}