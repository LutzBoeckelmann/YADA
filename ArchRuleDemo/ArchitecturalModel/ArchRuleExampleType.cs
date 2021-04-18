// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Test
{
    //MySystem.DomainLayer.Module.TechnicalLayer.Non.Architectural.Stuff


    //TechnicalLayer : Data | BusinessLogic | UI
    //DomainLayer: Infrastructure | Core | Extentions
    //ArchRuleExample.Infrastructure.Module1.Data.SubComponentHelper
    public class ArchRuleExampleType
    {
        public ArchRuleExampleType(ArchRuleDomainLayer dl, ArchRuleModule module, ArchRuleTechnicalLayer tl, bool valid)
        {
            DomainLayer = dl;
            Module = module;
            TechnicalLayer = tl;
            Valid = valid;
        }

        public ArchRuleDomainLayer DomainLayer {get;}
        public ArchRuleTechnicalLayer TechnicalLayer{get;}

        public ArchRuleModule Module { get; }


        public bool Valid { get; }
    }
}