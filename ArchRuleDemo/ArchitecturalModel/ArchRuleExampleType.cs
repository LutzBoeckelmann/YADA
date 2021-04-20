// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Test
{
 
    //ArchRuleExample.DomainLayer.Module.TechnicalLayer.Non.Architectural.Stuff


    //TechnicalLayer : Data | BusinessLogic | UI
    //DomainLayer: Infrastructure | Core | Extentions
    //ArchRuleExample.Infrastructure.Module1.Data.SubComponentHelper
    public class ArchRuleExampleType
    {

        public ArchRuleExampleType(string fullName, ArchRuleDomainLayer dl, ArchRuleModule module, ArchRuleTechnicalLayer tl, bool valid)
        {
            FullName = fullName;
            DomainLayer = dl;
            Module = module;
            TechnicalLayer = tl;
            Valid = valid;

        }

        public string FullName { get; }
        public ArchRuleDomainLayer DomainLayer {get;}
        public ArchRuleTechnicalLayer TechnicalLayer{get;}

        public ArchRuleModule Module { get; }


        public bool Valid { get; }

        public override string ToString()
        {
            return FullName;
        }
    }
}