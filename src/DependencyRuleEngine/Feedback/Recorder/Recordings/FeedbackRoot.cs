// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using YADA.Analyzer;


namespace YADA.DependencyRuleEngine.Feedback.Recorder.Recordings
{
    internal sealed class FeedbackRoot : IRecording
    {
        private readonly List<IRecording> m_Types = new List<IRecording>();

        private IRecording m_Current;

        public IDependencyContextVisitor<string> ContextVisitor { get; } = new ContextRecorder();

        public bool ReadMode { get; }

        public FeedbackRoot(bool readMode)
        {
            ReadMode = readMode;
        }

        internal List<TypeRecording> Types()
        {
            if (m_Current != null)
            {
                m_Current.Dispose();
                m_Current = null;
            }

            var types = m_Types.Cast<TypeRecording>().ToList();
            types.Reverse();
            return types;
        }

        void IRecording.Closed(IRecording current)
        {
            m_Types.Add(current);
        }
       
        public IDisposable Context(IDependencyContext context)
        {
            return m_Current.Context(context);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            return m_Current.ForbiddenDependency(dependency); 
        }

        public IDisposable Info(string msg)
        {
            return m_Current.Info(msg);
        }

        public IDisposable Type(string type)
        {
            if (m_Current != null)
            {
                m_Current.Dispose();

            }
            m_Current = new TypeRecording(type, this);

            return m_Current;
        }

        public IDisposable ViolatedRule(string rule)
        {
            return m_Current.ViolatedRule(rule);
        }
    }
}