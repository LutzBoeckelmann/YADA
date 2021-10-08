using ArchRuleExample.Core.CoreModule1.Data;

namespace ArchRuleExample.Infrastructure.InfraModule1.UI
{
    public class InfraModuleUIClass1
    {
        private Module1DataClass1 m_ForbiddenMember;

        public InfraModuleUIClass1(Module1DataClass1 forbiddenMember)
        {
            m_ForbiddenMember = forbiddenMember;
        }
    }
}