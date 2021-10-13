// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Text;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// A simple implementation of IFeedbackVisitor which
    /// prints the information into a string.
    /// </summary>
    public class ResultCollectorSimplePrinter : IFeedbackVisitor
    {
        private class Disposable : IDisposable
        {
            public void Dispose()
            {

            }
        }

        private readonly StringBuilder m_Result = new StringBuilder();
        private IDependencyContextVisitor<string> m_Visitor = new GenericDependencyContextVisitorSimplePrinter();
        public string GetFeedback()
        {
            return m_Result.ToString();
        }

        public IDisposable Context(IDependencyContext context)
        {
            m_Result.AppendLine($"    At: {context.Visit(m_Visitor)}");
            return new Disposable();
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            m_Result.AppendLine($"    ForbiddenDependency {dependency}");
            return new Disposable();
        }

        public IDisposable Info(string msg)
        {
            m_Result.AppendLine($"    Info: {msg}");
            return new Disposable();
        }

        public IDisposable ViolatedRule(string key)
        {
            m_Result.AppendLine($"  ViolatesRule: {key}");
            return new Disposable();
        }

        public IDisposable Type(string type)
        {
            m_Result.AppendLine($"Type: {type}");
            return new Disposable();
        }
    }
}