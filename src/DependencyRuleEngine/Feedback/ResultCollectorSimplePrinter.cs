// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// A simple implementation of IFeedbackVisitor which
    /// prints the information into a string.
    /// </summary>
    public class ResultCollectorSimplePrinter : IFeedbackVisitor
    {
        private sealed class Disposable : IDisposable
        {
            public void Dispose()
            {

            }
        }

        private readonly List<string> m_Result = new List<string>();

        private readonly IDependencyContextVisitor<string> m_Visitor = new GenericDependencyContextVisitorSimplePrinter();
        public string GetFeedback()
        {
            var builder = new StringBuilder();
            foreach(var line in m_Result)
            {
                builder.AppendLine(line);
            }
            return builder.ToString();
        }

        public IDisposable Context(IDependencyContext context)
        {
            m_Result.Add($"    At: {context.Visit(m_Visitor)}");
            return new Disposable();
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            m_Result.Add($"    ForbiddenDependency {dependency}");
            return new Disposable();
        }

        public IDisposable Info(string msg)
        {
            m_Result.Add($"    Info: {msg}");
            return new Disposable();
        }

        public IDisposable ViolatedRule(string rule)
        {
            m_Result.Add($"  ViolatesRule: {rule}");
            return new Disposable();
        }

        public IDisposable Type(string type)
        {
            m_Result.Add($"Type: {type}");
            return new Disposable();
        }
    }
}