using System.Collections.Generic;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal class DependencyRecording
    {
        public string DependencyName { get; set; }

        public List<ContextRecording> Contexts { get; set; } = new List<ContextRecording>();

        public void Add(ContextRecording item)
        {
            Contexts.Add(item);
        }
    }
}