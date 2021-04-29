// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace ArchRuleDemo.ArchitecturalModel
{
    public class ArchRuleDomainLayer : ArchRuleLayer<ArchRuleDomainLayer>
    {

        public static string Infrastructure => "Infrastructure";
        public static string Core => "Core";
        public static string Extentions => "Extentions";
        public ArchRuleDomainLayer(string layer) : base(layer, new List<string> { Infrastructure, Core, Extentions }) { }
    }
}