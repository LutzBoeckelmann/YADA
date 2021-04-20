using ArchRuleExample.Core.CoreModule1.Data;

namespace ArchRuleExample.Infrastructure.InfraModule1.Data
{
    public class InfraModuleDataClass1
    {
        private Module1DataClass1 m_ForbiddenMember;

        public InfraModuleDataClass1(Module1DataClass1 forbiddenMember)
        {
            m_ForbiddenMember = forbiddenMember;
        }
    }
}