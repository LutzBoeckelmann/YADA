using ArchRuleExample.Core.CoreModule1.Data;

namespace ArchRuleExample.Core.CoreModule1.UI
{
    public class View1
    {
        readonly Module1DataClass1 m_AllowedAccess;
        public Module1DataClass1 AllowedAccess => m_AllowedAccess;

        public View1(Module1DataClass1 allowedAccess)
        {
            m_AllowedAccess = allowedAccess;
        }
    }
}