// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using YADA.Analyzer;

namespace YADA.ArchGuard.BuildingBlock
{

    public interface ILogicalExpression
    {
        bool Evaluate(ITypeDescription typeDescription);
    }

    public interface IBuildingBlockTypeFilter
    {
        bool Match(Analyzer.ITypeDescription typeDefintion);

        string[] AsString { get; }
    }

    public class BuildingBlockTypeFilter : IBuildingBlockTypeFilter
    {
        private ILogicalExpression m_Matcher;
        private string[] m_AsString; 

        public BuildingBlockTypeFilter(string filter) : this(new List<string>() { filter }) { }

        public BuildingBlockTypeFilter(IEnumerable<string> filters)
        {
            var parser = new ShiftReduceParser();
            m_Matcher = parser.ParseLogicalExpression(filters.ToList());
            m_AsString = filters.ToArray();
        }

        public string[] AsString => m_AsString;

        /// <summary>
        /// not and or regex
        /// </summary>
        /// <param name="typeDefintion"></param>
        /// <returns></returns>
        public bool Match(ITypeDescription typeDefintion)
        {
            return m_Matcher.Evaluate(typeDefintion);
        }
    }
    /*
     BuildingBlockDescription
        name:
        filter:
     */
    public class BuildingBlockDescription : IBuildingBlockDescription
    {
        public BuildingBlockDescription(string name, IBuildingBlockTypeFilter filter/*, bool isAbstract*/)
        {
            Name = name;
            TypeFilter = filter;
            //Abstract = isAbstract;
        }

        public string Name { get; }

        /// <summary>
        /// Indicates if the BuildingBlock may contain a class or not.
        /// 
        /// For example a layer box may not contain a type directly any type needs to be part of a child layer.
        /// </summary>
        [Obsolete("Not any longer supported. Any buildingblock with children is abstract")]
        public bool Abstract { get; }

        public IBuildingBlockTypeFilter TypeFilter { get; }
    }
}


