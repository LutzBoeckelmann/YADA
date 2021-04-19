// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Test
{
    //MySystem.DomainLayer.Module.TechnicalLayer.Non.Architectural.Stuff


    //TechnicalLayer : Data | BusinessLogic | UI
    //DomainLayer: Infrastructure | Core | Extentions
    //ArchRuleExample.Infrastructure.Module1.Data.SubComponentHelper

    public class ArchRuleExampleTypeRepository : IArchRuleExampleTypeRepository
    {
        private readonly Dictionary<string, ArchRuleExampleType> m_Types = new Dictionary<string, ArchRuleExampleType>();
        
        public ArchRuleExampleType GetTypeRepresentation(string fullqualifiedType) 
        {
            if (!m_Types.TryGetValue(fullqualifiedType, out ArchRuleExampleType result))
            {
                var isValid = true;
                var namespaceParts = fullqualifiedType.Split('.');
                if (namespaceParts[0] != "MySystem")
                {
                    isValid = false;
                }

                var dl = new ArchRuleDomainLayer(namespaceParts[1]);
                isValid &= dl.Valid;
                var module = new ArchRuleModule(namespaceParts[2]);

                var tl = new ArchRuleTechnicalLayer(namespaceParts[3]);
                isValid &= tl.Valid;
                result = new ArchRuleExampleType(fullqualifiedType, dl, module, tl, isValid);

                m_Types.Add(fullqualifiedType, result);
            }

            return result;
        }
    }
}