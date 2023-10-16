// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback.Recorder.Recordings
{
    internal class TypeRecording : IRecording
    {
        private readonly IRecording m_Parent;
        private RuleRecording m_Current;

        public TypeRecording(string name, IRecording parent)
        {
            TypeName = name;
            m_Parent = parent;
        }
        public string TypeName { get; }

        public List<RuleRecording> Rules { get; set; } = new List<RuleRecording>();

        public IDependencyContextVisitor<string> ContextVisitor => m_Parent.ContextVisitor;
        public bool ReadMode => m_Parent.ReadMode;

        public void Closed(IRecording typeRecording)
        {
            if(typeRecording != m_Current || m_Current == null) 
            {
                throw new NotSupportedException();
            }
           
            Rules.Add(m_Current);
            m_Current = null;
        }

        public IDisposable Context(IDependencyContext context)
        {
            return m_Current.Context(context);
        }

        public void Dispose()
        {
            if(m_Current != null) 
            {
                m_Current.Dispose();
            }
            
            m_Parent.Closed(this);
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
            throw new NotImplementedException();
        }

        public IDisposable ViolatedRule(string rule)
        {
            
            // m_Current must be null
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

            m_Current = new RuleRecording(rule, this);
                        
            return m_Current;
        }
    }
}