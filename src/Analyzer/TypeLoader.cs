// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace YADA.Analyzer
{
    /// <summary>
    /// Analyses all types and there dependencies.
    /// </summary>
    public class TypeLoader
    {
        private sealed class MultipleTypeFilter : ITypeFilter
        {
            private readonly IList<ITypeFilter> m_InternalFilters;

            public MultipleTypeFilter(IEnumerable<ITypeFilter> internalFilters)
            {
                m_InternalFilters = internalFilters.ToList();
            }

            public bool IgnoreType(string typeFullname)
            {
                return m_InternalFilters.Any(f => f.IgnoreType(typeFullname));
            }

            public bool IgnoreTypeAsDependency(string typeFullname)
            {
                return m_InternalFilters.Any(f => f.IgnoreTypeAsDependency(typeFullname));
            }
        }
        private readonly TypeAnalyser m_TypeAnalyser;
        private readonly List<string> m_AssemblyLocations;
        private List<ITypeDescription> m_Types;
        private readonly MultipleTypeFilter m_TypeFilter;

        /// <summary>
        /// Constructs a TypeLoader. 
        /// 
        /// The constructor determines all IgnoreAttributes from the StackTrace.
        /// The method uses the diagnostic service which may not be available
        /// in any builds. In doubt collect the attributes directly and provide
        /// them as parameter.
        /// </summary>
        /// <param name="locations">Locations of the assemblies which should be analyzed</param>
        public TypeLoader(IEnumerable<string> locations) : this(locations, GetIgnoreAttributes()) { }

        /// <summary>
        /// Constructs a TypeLoader
        /// 
        /// Load the IgnoreAttributes from the given methods (The methods are the test methods creating
        /// this TypeLoader carrying the IngoreTypeAttributes.         
        /// </summary>
        /// <param name="locations">Locations of the assemblies which should be analyzed</param>
        /// <param name="callStack"></param>
        public TypeLoader(IEnumerable<string> locations, IReadOnlyCollection<MethodBase> callStack) : this(locations, GetIgnoreAttributes(callStack)) 
        { }
        
        private TypeLoader(IEnumerable<string> locations, IReadOnlyCollection<IgnoreTypeAttribute> ignoreAttributes)
        {
            m_TypeFilter = new MultipleTypeFilter(ReadIgnoreAttributes(ignoreAttributes));

            m_AssemblyLocations = locations.ToList();
            m_TypeAnalyser = new TypeAnalyser(m_TypeFilter);
        }

        /// <summary>
        /// This method determines all IgnoreAttributes from the StackTrace.
        /// The method uses the diagnostic service which may not be available
        /// in any builds. In doubt collect the attributes directly and provide
        /// them as parameter.
        /// </summary>
        /// <returns></returns>
        private static IReadOnlyCollection<IgnoreTypeAttribute> GetIgnoreAttributes()
        {
            return GetIgnoreAttributes(new StackTrace().GetFrames().Select(f => f.GetMethod()).ToArray());
        }

        private static IReadOnlyCollection<IgnoreTypeAttribute> GetIgnoreAttributes(IReadOnlyCollection<MethodBase> callStack)
        {
            List<IgnoreTypeAttribute> result = new List<IgnoreTypeAttribute>();
            foreach (var method in callStack)
            {
                result.AddRange(method.GetCustomAttributes(typeof(IgnoreTypeAttribute), true)?.Cast<IgnoreTypeAttribute>());
            }

            return result;
        }

        private static IReadOnlyCollection<ITypeFilter> ReadIgnoreAttributes(IEnumerable<IgnoreTypeAttribute> list)
        {
            var result = new List<ITypeFilter>();

            if (list != null)
            {
                foreach (var ignoreAttribute in list)
                {
                    if (ignoreAttribute.PatternType == IgnorePatternType.Glob)
                    {
                        result.Add(new GlobTypeFilter(ignoreAttribute.Pattern, ignoreAttribute.IgnoreTypeAsDependency));
                    }
                    else if (ignoreAttribute.PatternType == IgnorePatternType.Regex)
                    {
                        result.Add(new RegExMatcher(ignoreAttribute.Pattern, ignoreAttribute.IgnoreTypeAsDependency));
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// All types found in the given assemblies. The types are represented as
        /// ITypeDescription. Start point of the analyze.
        /// </summary>
        public IEnumerable<ITypeDescription> GetTypes()
        {
            if (m_Types == null)
            {
                m_Types = new List<ITypeDescription>();
                foreach (var location in m_AssemblyLocations)
                {
                    m_Types.AddRange(GetTypesInternal(location));
                }

            }
            return m_Types;
        }

        private IEnumerable<TypeDescription> GetTypesInternal(string location)
        {
            foreach (var type in GetTypesFromAssembly(location, m_TypeFilter))
            {
                yield return m_TypeAnalyser.AnalyseType(type);
            }
        }

        private static IEnumerable<TypeDefinition> GetTypesFromAssembly(string location, ITypeFilter filter = null)
        {
            var assembly = AssemblyDefinition.ReadAssembly(location);

            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.GetTypes())
                {
                    if (type.FullName != "<Module>" && (filter == null || !filter.IgnoreType(type.FullName)))
                    {
                        yield return type;
                    }
                }
            }
        }

        /// <summary>
        /// An interface primary for test purposes. Delivers a description for the given fullName.
        /// The string based argument should enable the access to the functionality of a single type 
        /// without the need to reference mono cecil.
        /// </summary>
        /// <param name="fullName">The full qualified name of the type</param>
        /// <returns>The description of the type</returns>
        public static ITypeDescription GetType(string fullName, string assemblyLocation)
        {
            var type = GetTypesFromAssembly(assemblyLocation).First(t => t.FullName == fullName);
            return new TypeAnalyser().AnalyseType(type);
        }
    }
}
