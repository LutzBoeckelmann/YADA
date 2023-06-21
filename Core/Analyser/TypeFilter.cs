// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Text.RegularExpressions;
using Mono.Cecil;

namespace YADA.Core.Analyser
{
    internal interface ITypeFilter
    {
        bool IgnoreType(TypeDefinition type);
        bool IgnoreTypeAsDependency(TypeDefinition type);
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
        public bool IgnoreType(TypeDefinition type)
        {
            return m_Matcher.IsMatch(type.FullName);
        }

        public bool IgnoreTypeAsDependency(TypeDefinition type)
        {
            return m_IgnoreAlsoAsDependencies && m_Matcher.IsMatch(type.FullName);
        }
    }


    internal class RegExMatcher : ITypeFilter
    {
        private readonly Regex m_RegEx;
        private readonly bool m_IgnoreAlsoAsDependencies;

        public RegExMatcher(string regex, bool ignoreAlsoAsDependencies) 
        {
            m_RegEx = new Regex(regex, RegexOptions.Compiled);
            m_IgnoreAlsoAsDependencies = ignoreAlsoAsDependencies;
        }
        public bool IgnoreType(TypeDefinition type)
        {
            return m_RegEx.IsMatch(type.FullName);
        }

        public bool IgnoreTypeAsDependency(TypeDefinition type)
        {
            return m_IgnoreAlsoAsDependencies && m_RegEx.IsMatch(type.FullName);
        }
    }
}
