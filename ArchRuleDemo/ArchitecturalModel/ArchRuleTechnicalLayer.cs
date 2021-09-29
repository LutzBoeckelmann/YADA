// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using Core.DependencyRuleEngine.Rules;

namespace ArchRuleDemo.ArchitecturalModel
{
    public class ArchRuleTechnicalLayer : BaseLayer<ArchRuleTechnicalLayer>
    {
        public static string Data => "Data";
        public static string BusinessLogic => "BusinessLogic";
        public static string UI => "UI";

        private static List<string> s_Layers = new List<string> { Data, BusinessLogic, UI    };

        protected override List<string> Layers => s_Layers;

        public ArchRuleTechnicalLayer(string layer) : base(layer)
        {
            Layer = layer;
        }
        public string Layer { get; }
        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}