// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Text;
using System.Text.RegularExpressions;

namespace YADA.Analyzer
{
    /// <summary>
    /// A simple class to match a string against a Glob Pattern
    /// In this case it matches namespaces
    /// a single * matches any character except '.'
    /// a ** matches also the '.'
    /// </summary>
    internal class GlobPatternMatcher
    {
        private readonly Regex m_Pattern;

        public GlobPatternMatcher(string pattern)
        {
            StringBuilder patternBuilder = new StringBuilder();
                        
            for(int i=0;i<pattern.Length; i++)
            {
                if(pattern[i] == '.')
                {
                    patternBuilder.Append("\\.");
                }
                else if(pattern[i] == '*')
                {
                    if(i<pattern.Length-1 && pattern[i+1] == '*')
                    {
                        i++;
                        patternBuilder.Append(".*");
                    }
                    else
                    {
                        patternBuilder.Append("[^.]*");
                    }
                } else
                {
                    patternBuilder.Append(pattern[i]);
                }
            }
            
            m_Pattern = new Regex(patternBuilder.ToString(), RegexOptions.None, System.TimeSpan.FromMilliseconds(100));
        }

        public bool IsMatch(string input)
        {
            return m_Pattern.IsMatch(input);
        }
    }
}
