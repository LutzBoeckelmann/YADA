// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Text.RegularExpressions;

namespace YADA.Analyzer
{
    internal interface ITypeFilter
    {
        bool IgnoreType(string type);
        bool IgnoreTypeAsDependency(string typeFullName);
    }

    internal class GlobTypeFilter : ITypeFilter
    {
        private readonly GlobPatternMatcher m_Matcher;
        private readonly bool m_IgnoreAlsoAsDependencies;
        public GlobTypeFilter(string globPattern, bool ignoreAlsoAsDependencies)
        {
            m_Matcher = new GlobPatternMatcher(globPattern);
            m_IgnoreAlsoAsDependencies = ignoreAlsoAsDependencies;
        }
        public bool IgnoreType(string typeFullName)
        {
            return m_Matcher.IsMatch(typeFullName);
        }

        public bool IgnoreTypeAsDependency(string typeFullName)
        {
            return m_IgnoreAlsoAsDependencies && m_Matcher.IsMatch(typeFullName);
        }
    }


    internal class RegExMatcher : ITypeFilter
    {
        private readonly Regex m_RegEx;
        private readonly bool m_IgnoreAlsoAsDependencies;

        public RegExMatcher(string regex, bool ignoreAlsoAsDependencies) 
        {
            m_RegEx = new Regex(regex, RegexOptions.Compiled, System.TimeSpan.FromMilliseconds(100));
            m_IgnoreAlsoAsDependencies = ignoreAlsoAsDependencies;
        }
        public bool IgnoreType(string typeFullName)
        {
            return m_RegEx.IsMatch(typeFullName);
        }

        public bool IgnoreTypeAsDependency(string typeFullName)
        {
            return m_IgnoreAlsoAsDependencies && m_RegEx.IsMatch(typeFullName);
        }
    }
}
