// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public class FeedbackFilter : IFeedbackVisitor
    {   
        private class Callback : IDisposable
        {
            private readonly Action m_Callback;

            public Callback(Action callback)
            {
                m_Callback = callback;
            }

            public void Dispose()
            {
                m_Callback();
            }
        }
        private readonly List<TypeRecording> m_Types;

        private readonly IFeedbackVisitor m_Next;
        private TypeRecording m_CurrentType;
        private DependencyRecording m_CurrentDependency;
        private RuleRecording m_CurrentRule;
        private readonly ContextRecorder m_Context = new ContextRecorder();

        private void NotifyType() 
        {
            if(m_TypeNotNotified) 
            {
                m_NextTypeDisposable = m_Next.Type(m_CurrentType.TypeName);
            }
            m_TypeNotNotified = false;
        }
        private bool m_TypeNotNotified;

        private IDisposable m_NextTypeDisposable;

        private void NotifyRule() 
        {
            NotifyType();

            if(m_RuleNotNotified) 
            {
                m_NextRuleDisposable = m_Next.ViolatedRule(m_CurrentRule.RuleName);
            }
            m_RuleNotNotified = false;
        }
        private bool m_RuleNotNotified;
        private IDisposable m_NextRuleDisposable;
        private void NotifyDependency() 
        {
            NotifyRule();
            if(m_DependencyNotNotified) 
            {
                m_NextDependencyDisposable = m_Next.ForbiddenDependency(m_CurrentDependency.DependencyName);
            }
            m_DependencyNotNotified = false;
        }

        private bool m_DependencyNotNotified;
        private IDisposable m_NextDependencyDisposable;
        
        public FeedbackFilter(string file, IFeedbackVisitor next) 
        {
            var reader = new FeedbackReader();
            reader.ReadRecording(file);
            m_Types = reader.Types();

            m_Next = next;
        }

        internal FeedbackFilter(IEnumerable<TypeRecording> types, IFeedbackVisitor next)
        {
            m_Types = types.ToList();
            m_Next = next;
        }

        public IDisposable Context(IDependencyContext context)
        {
            IDisposable nextContextDisposable = null;
            ContextRecording currentContext = null;
            
            if (m_CurrentDependency != null)
            {
                currentContext = m_CurrentDependency.Contexts.FirstOrDefault(t => t.Context == context.Visit(m_Context));
            }

            if(currentContext == null)
            {
                NotifyDependency();
                nextContextDisposable = m_Next.Context(context);
            }

            return new Callback(() => { nextContextDisposable?.Dispose(); });
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            if(m_CurrentRule != null) 
            {
                m_CurrentDependency = m_CurrentRule.Dependencies.FirstOrDefault(t => t.DependencyName == dependency);
            }

            if(m_CurrentDependency != null) 
            {
                m_DependencyNotNotified = true;
            }
            else 
            {
                NotifyRule();
                m_NextDependencyDisposable = m_Next.ForbiddenDependency(dependency);
            }

            return new Callback(() => { m_DependencyNotNotified = false; m_CurrentDependency = null; m_NextDependencyDisposable?.Dispose(); });
        }

        public IDisposable Info(string msg)
        {
            IDisposable disposable = null;
            InfoRecording currentInfo = null;
            if(m_CurrentRule != null) 
            {
                currentInfo = m_CurrentRule.Infos.FirstOrDefault(t => t.Info == msg);
            }
            else
            {
                // do ignore unknown infos for known rules!
                disposable = m_Next.Info(msg);
            }
            
            return new Callback(() => { disposable?.Dispose(); });
        }

     
        public IDisposable Type(string type)
        {
            m_CurrentType = m_Types.FirstOrDefault(t => t.TypeName == type);
            
            if(m_CurrentType == null) 
            {
                m_NextTypeDisposable = m_Next.Type(type);
            }
            else 
            {
                m_TypeNotNotified = true;
            }

            return new Callback(
                () => { m_CurrentType = null; m_TypeNotNotified = false; m_NextTypeDisposable?.Dispose(); });
        }

        public IDisposable ViolatedRule(string rule)
        {
            if(m_CurrentType != null) 
            {
                m_CurrentRule = m_CurrentType.Rules.FirstOrDefault(t => t.RuleName == rule);
            }
            
            if(m_CurrentRule == null) 
            {
                NotifyType();
                m_NextRuleDisposable = m_Next.ViolatedRule(rule);
            }
            else
            {
                m_RuleNotNotified = true;
            }

            return new Callback(
                () => { m_CurrentRule = null; m_RuleNotNotified = false; m_NextRuleDisposable?.Dispose(); });
        }
    }
}