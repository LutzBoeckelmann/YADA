// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using Core.DependencyRuleEngine.Rules;

namespace ArchRuleDemo.ArchitecturalModel
{
    public class ArchRuleDomainLayer : BaseLayer<ArchRuleDomainLayer>
    {
        public static string Infrastructure => "Infrastructure";
        public static string Core => "Core";
        public static string Extensions => "Extensions";
        private static List<string> s_Layers = new List<string> { Infrastructure, Core, Extensions };
        protected override List<string> Layers => s_Layers;

        public ArchRuleDomainLayer(string layer)  : base (layer)
        {
            
        }
    }
}