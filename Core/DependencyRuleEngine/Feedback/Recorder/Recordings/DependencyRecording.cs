// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal class DependencyRecording : IRecording
    {
        private readonly IRecording m_Parent;

        public DependencyRecording(string name, IRecording parent) 
        {
            m_Parent = parent;
            DependencyName = name;
        }
        public bool ReadMode => m_Parent.ReadMode;
        public string DependencyName { get; }

        public List<ContextRecording> Contexts { get; } = new List<ContextRecording>();

        public IDependencyContextVisitor<string> ContextVisitor => m_Parent.ContextVisitor;

        public void Closed(IRecording current)
        {
            throw new NotSupportedException();
        }

        public IDisposable Context(IDependencyContext context)
        {
            if (context is RecordedContext recording)
            {
                return InternalContext(recording.SerializedContext);
            }

            return InternalContext(context.Visit(ContextVisitor));
        }

        private IDisposable InternalContext(string context) 
        {
            var result = new ContextRecording(context, this);

            Contexts.Add(result);

            return result;
        }

        public void Dispose()
        {
            m_Parent.Closed(this);
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
    }
}