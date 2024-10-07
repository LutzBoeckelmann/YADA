// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

using YADA.Analyzer;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard
{
    public class DependencyChecker
    {
        private readonly TypeMatcher m_TypeMatcher;

        public DependencyChecker(TypeMatcher typeMatcher)
        {
            m_TypeMatcher = typeMatcher;
        }

        public bool Check(ITypeDescription type, ITypeDescription dependency, ICheckFeedback feedback)
        {
            var connectingChain = new List<IBuildingBlock>(m_TypeMatcher.GetConnectingChain(type, dependency));
            bool result = true;
            for (int i = 0; i < connectingChain.Count; i++)
            {
                result &= connectingChain[i].Check(connectingChain, feedback);
            }
                        
            return result;
        }
    }
}

