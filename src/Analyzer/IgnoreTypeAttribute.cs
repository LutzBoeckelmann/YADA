// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;

namespace YADA.Analyzer
{

    public enum IgnorePatternType
    {
        Glob,
        Regex
    }

    public enum IgnoreTypeOptions
    {
        None,
        IgnoreAlsoAsDependency
    }

    /// <summary>
    /// This attribute can be used to configure types which should be ignored.
    /// This works on fully qualified names and ignores assemblies at the moment.
    /// The types can be selected by a glob or regex pattern.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class IgnoreTypeAttribute : Attribute
    {
        /// <summary>
        /// Constructs a filter to ignore types. The pattern is interpreted as Glob.
        /// In this case * matches any character except .
        /// The ** matches anything.
        /// </summary>
        /// <param name="pattern">Pattern of the types to ignore.</param>
        public IgnoreTypeAttribute(string pattern) : this(pattern, IgnorePatternType.Glob)
        {
            
        }

        /// <summary>
        /// Constructs a filter to ignore types.
        /// </summary>
        /// <param name="pattern">Pattern of the types to ignore. Could be a Glob or a RegEx</param>
        /// <param name="patternType">Type of the pattern, Glob or Regex. Default is glob</param>
        public IgnoreTypeAttribute(string pattern, IgnorePatternType patternType) : this(pattern, patternType, IgnoreTypeOptions.IgnoreAlsoAsDependency)
        {
            
        }

        /// <summary>
        /// Constructs a filter to ignore types.
        /// </summary>
        /// <param name="pattern">Pattern of the types to ignore. The pattern will be interpreted as glob</param>
        /// <param name="ignoreTypeAsDependency">Ignore the matched types also if the appear as dependency of an analyzed type</param>
        public IgnoreTypeAttribute(string pattern, IgnoreTypeOptions ignoreTypeAsDependency) : this(pattern,IgnorePatternType.Glob, ignoreTypeAsDependency)
        {
            
        }

        /// <summary>
        /// Constructs a filter to ignore types.
        /// </summary>
        /// <param name="pattern">Pattern of the types to ignore. Could be a Glob or a RegEx</param>
        /// <param name="patternType">Type of the pattern, Glob or Regex. Default is glob</param>
        /// <param name="ignoreTypeAsDependency">Ignore the matched types also if the appear as dependency of an analyzed type</param>
        public IgnoreTypeAttribute(string pattern, IgnorePatternType patternType, IgnoreTypeOptions ignoreTypeAsDependency)
        {
            Pattern = pattern;
            PatternType = patternType;
            IgnoreTypeAsDependency = ignoreTypeAsDependency == IgnoreTypeOptions.IgnoreAlsoAsDependency ;
        }


        /// <summary>
        /// Type of the pattern, Glob or Regex
        /// </summary>
        public IgnorePatternType PatternType { get; }
        
        /// <summary>
        /// Indicates if an ignored type should also be ignored as dependency
        /// of other types (suppressed)
        /// </summary>
        internal bool IgnoreTypeAsDependency { get; }

        /// <summary>
        /// The pattern to ignore
        /// </summary>
        internal string Pattern { get; }

        
    }
}
