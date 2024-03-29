// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using NUnit.Framework;
using YADA.Analyzer;

namespace YADA.AnalyzerTests
{
    [TestFixture]
    public class FirstSimpleExample
    {
        [Explicit]
        [Test]
        [IgnoreType("**.*Dependency*")]
        public void Simple_Way_To_List_All_Dependencies_In_ExampleDll() 
        {
            // creating the type loader and load all types from the example dll
            var typeLoader = new TypeLoader(new[] { @"./YADA.Example.dll"});

            // fetch all types 
            var typeDescriptions = typeLoader.GetTypes();

            // iterate over all type for the analyze 
            foreach (var typeDescription in typeDescriptions)
            {
                // access the full qualified name of the current type
                Console.WriteLine($"Type: {typeDescription.FullName}");
                Console.WriteLine("  Dependencies:");

                //iterate over all dependencies of the current type
                foreach (var typeDependency in typeDescription.Dependencies)
                {
                    // access the full qualified name of the dependencies type
                    Console.WriteLine($"    - {typeDependency.Type.FullName}");
                }
            }
        }
    }
}