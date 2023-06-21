using ArchRuleExample.Core.CoreModule1.Data;

namespace ArchRuleExample.Infrastructure.InfraModule1.UI
{
    public class InfraModuleUIClass1
    {
        private readonly Module1DataClass1 m_ForbiddenMember;

        public InfraModuleUIClass1(Module1DataClass1 forbiddenMember)
        {
            m_ForbiddenMember = forbiddenMember;
        }
    }

    public class InfraModuleUIClass2
    {
        private readonly Module1DataClass1 m_ForbiddenMember;

        public InfraModuleUIClass2(Module1DataClass1 forbiddenMember)
        {
            m_ForbiddenMember = forbiddenMember;
        }
    }

    public class InfraModuleUIClass3
    {
        private readonly InfraModuleUIClass2 m_OkTest;

        public InfraModuleUIClass3(InfraModuleUIClass2 forbiddenMember)
        {
            m_OkTest = forbiddenMember;
        }
    }
}