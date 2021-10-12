// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Text;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// A simple implementation of IFeedbackVisitor which
    /// prints the information into a string.
    /// </summary>
    public class ResultCollectorSimplePrinter : IFeedbackVisitor
    {
        private readonly StringBuilder m_Result = new StringBuilder();

        public string GetFeedback()
        {
            return m_Result.ToString();
        }

        public void Context(string context)
        {
            m_Result.AppendLine($"    At: {context}");
        }

        public void ForbiddenDependency(string dependency)
        {
            m_Result.AppendLine($"    ForbiddenDependency {dependency}");
        }

        public void Info(string msg)
        {
            m_Result.AppendLine($"    Info: {msg}");
        }

        public void ViolatedRule(string key)
        {
            m_Result.AppendLine($"  ViolatesRule: {key}");
        }

        public void Type(string type)
        {
            m_Result.AppendLine($"Type: {type}");
        }
    }
}