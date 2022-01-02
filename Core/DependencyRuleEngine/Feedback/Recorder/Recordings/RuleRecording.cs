using System.Collections.Generic;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal class RuleRecording
    {
        public string RuleName { get; set; }

        public List<DependencyRecording> Dependencies { get; set; } = new List<DependencyRecording>();
        public List<InfoRecording> Infos { get; set; } = new List<InfoRecording>();

        public void Add(DependencyRecording item)
        {
            Dependencies.Add(item);
        }

        internal void AddInfo(InfoRecording infoRecording)
        {
            Infos.Add(infoRecording);
        }
    }
}