// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace ArchRuleDemo.ArchitecturalModel
{
    //ArchRuleExample.DomainLayer.Module.TechnicalLayer.Non.Architectural.Stuff


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


                if (namespaceParts[0] != "ArchRuleExample")
                {
                    isValid = false;
                }
                var domainLayerName = namespaceParts.Length > 1 ? namespaceParts[1] : "";
                var dl = new ArchRuleDomainLayer(domainLayerName);
                isValid &= dl.Valid;

                var moduleName = namespaceParts.Length > 2 ? namespaceParts[2] : "";
                var module = new ArchRuleModule(moduleName);

                var technicalLayerName = namespaceParts.Length > 3 ? namespaceParts[3] : "";
                var tl = new ArchRuleTechnicalLayer(technicalLayerName);
                isValid &= tl.Valid;
                result = new ArchRuleExampleType(fullqualifiedType, dl, module, tl, isValid);

                m_Types.Add(fullqualifiedType, result);
            }

            return result;
        }
    }
}