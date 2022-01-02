using System.Collections.Generic;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal class TypeRecording
    {
        public string TypeName { get; set; }

        public List<RuleRecording> Rules { get; set; } = new List<RuleRecording>();

        public void Add(RuleRecording item)
        {
            Rules.Add(item);
        }
    }
}