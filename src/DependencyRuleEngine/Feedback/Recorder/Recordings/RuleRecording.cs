// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback.Recorder.Recordings
{
    internal class RuleRecording : IRecording
    {
        private readonly IRecording m_Parent;
        private DependencyRecording m_Current;
        public RuleRecording(string name, IRecording parent)
        {
            RuleName = name;
            m_Parent = parent;
        }
        public bool ReadMode => m_Parent.ReadMode;
        public string RuleName { get; }

        public List<DependencyRecording> Dependencies { get; } = new List<DependencyRecording>();
        public List<InfoRecording> Infos { get; } = new List<InfoRecording>();

        public IDependencyContextVisitor<string> ContextVisitor => m_Parent.ContextVisitor;

        public void Closed(IRecording current)
        {
            if(m_Current != current) {
                throw new NotSupportedException();
            }
            
            m_Current = null;
        }

        public IDisposable Context(IDependencyContext context)
        {
            return m_Current.Context(context);
        }

        public void Dispose()
        {
            m_Parent.Closed(this);
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            if(m_Current != null)
            {
                if (!ReadMode)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    m_Current.Dispose();
                }
            }
            m_Current =  new DependencyRecording(dependency, this);
            Dependencies.Add(m_Current);
            return m_Current;
        }

        public IDisposable Info(string msg)
        {
            var info = new InfoRecording(msg, this);
            Infos.Add(info);
            return info;
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