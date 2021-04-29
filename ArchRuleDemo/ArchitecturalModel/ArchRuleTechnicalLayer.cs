// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace ArchRuleDemo.ArchitecturalModel
{
    public class ArchRuleTechnicalLayer : ArchRuleLayer<ArchRuleTechnicalLayer>
    {

        public static string Data => "Data";
        public static string BusinessLogic => "BusinessLogic";
        public static string UI => "UI";

        public ArchRuleTechnicalLayer(string layer) : base(layer, new List<string> { Data, BusinessLogic, UI }) { }
    }
}