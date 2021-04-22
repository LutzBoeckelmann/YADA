// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace YADA.Core
{

    public interface ITypeDescription
    {

        string FullName { get; }

        IEnumerable<IDependency> Dependencies { get; }

    }

    public class TypeLoader
    {

        private readonly List<string> m_AssemblyLocations;
        private List<ITypeDescription> m_Types;

        public TypeLoader(IEnumerable<string> locations)
        {
            m_AssemblyLocations = locations.ToList();
        }

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

        private static IEnumerable<TypeDescription> GetTypesInternal(string location)
        {
            foreach (var type in GetTypesFromAssembly(location))
            {
                yield return TypeAnalyser.AnalyseType(type);
            }
        }

        private static IEnumerable<TypeDefinition> GetTypesFromAssembly(string location)
        {
            var assembly = AssemblyDefinition.ReadAssembly(location);

            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.GetTypes())
                {
                    if (type.FullName != "<Module>")
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
        public static TypeDescription GetType(string fullName, string assemblyLocation)
        {
            var type = GetTypesFromAssembly(assemblyLocation).First(t => t.FullName == fullName);
            return TypeAnalyser.AnalyseType(type);
        }

    

    }
}
