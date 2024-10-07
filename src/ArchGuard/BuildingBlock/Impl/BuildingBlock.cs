// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using YADA.Analyzer;
using YADA.ArchGuard.Behavior;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.BuildingBlock.Impl
{
    /// <summary>
    /// Responsibilities
    /// 
    /// 1) hold the description of all building blocks as tree 
    /// 2) Create the structure
    /// 3) Find a chain of BuildingBlock for a given type definition
    ///    Question
    /// Todo introduce a blockage between both usages. After creation
    /// the BuildingBlocks are readonly.
    /// </summary>

    public class BuildingBlock : IBuildingBlock
    {
        private List<BuildingBlock> m_Children = new List<BuildingBlock>();
        public IBuildingBlock Parent { get; }

        public IBuildingBlockBehavior Behavior { get; }
        public IBuildingBlockDescription Description { get; }
        public IReadOnlyList<IBuildingBlock> Children => m_Children;

        public BuildingBlock(IBuildingBlock parent, IBuildingBlockDescription description, IBuildingBlockBehavior behavior)
        {
            Parent = parent;
            Description = description;
            Behavior = behavior;
        }

        public BuildingBlock AddChild(IBuildingBlockDescription dependency, IBuildingBlockBehavior behavior)
        {
            if (!Description.Abstract)
            {
                throw new NotSupportedException("Can not add a child building block to a non abstract building block");
            }

            var child = new BuildingBlock(this, dependency, behavior);
            m_Children.Add(child);
            return child;
        }

        public BuildingBlock Clone(BuildingBlock parent)
        {
            var clone = new BuildingBlock(parent, Description, Behavior);
                        
            clone.m_Children.AddRange(m_Children.Select(i => i.Clone(this)));

            return clone;
        }


        //maybe an own class as result containing more than the results

        /// <summary>
        /// Returns a collection of building blocks containing the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IReadOnlyCollection<IBuildingBlock> GetChain(ITypeDescription type)
        {
            List<IBuildingBlock> result = new List<IBuildingBlock>();

            InternalGetChain(type, result);

            result.Reverse();

            if (result.Count == 0 || result.First().Description.Abstract)
            {
                // error
                return null; // better error indication needed
            }

            return result;
        }

        private bool InternalGetChain(ITypeDescription type, List<IBuildingBlock> chain)
        {
            if (Match(type))
            {
                chain.Add(this);

                foreach (var child in m_Children)
                {
                    if (child.InternalGetChain(type, chain))
                    {
                        break;
                    }
                }

                return true;
            }
            return false;
        }

        private bool Match(ITypeDescription description)
        {
            return Description.TypeFilter.Match(description);
        }

        public override string ToString()
        {
            return Description.Name;
        }

        public bool Check(IReadOnlyList<IBuildingBlock> chain, ICheckFeedback feedback)
        {
            int currentIndex = -1;
            int counter = 0;
            foreach (var block in chain)
            {

                if(block == this)
                {
                    currentIndex = counter;
                }
                counter++;
            }
            // At the moment I find no example how more than one rule have to be applied
            // but it feels not correct to skip all after a rule was violated
            // an use case could be a "rule" which adds only information

            var result = true;

            result &= Behavior.ContainerBehavior.Check(chain, currentIndex, this, feedback);

            result &= Behavior.InternalBehavior.Check(chain, currentIndex, feedback);

            return result;
        }
        
        public int GetIndex(IBuildingBlock buildingBlock)
        {
            if (buildingBlock.Parent != this)
            {
                throw new InvalidOperationException();
            }

            return m_Children.IndexOf(buildingBlock as BuildingBlock);
        }

    }
}

