// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace Core.DependencyRuleEngine.Rules
{
    /// <summary>
    /// A base class which may be used to create specific represent of layers. 
    /// The class is abstract and generic so a specific layer represented by
    /// the derived class can be used type save. 
    /// </summary>
    public abstract class BaseLayer<T> where T:BaseLayer<T>
    {
        /// <summary>
        /// Indicates whether this is a valid layer in the system. 
        /// </summary>
        /// <value></value>
        public bool Valid 
        { 
            get 
            {
                if(!m_IsValidEvaluated)
                {
                    m_Valid = Layers.Contains(m_Layer);
                    m_IsValidEvaluated = true;
                }
        
                return m_Valid;
            } 
        }

        private bool m_IsValidEvaluated = false;
        private bool m_Valid;
        /// <summary>
        /// Name of the layer 
        /// </summary>
        private readonly string m_Layer;

        /// <summary>
        /// List of all layers. The order in the list is the order of the layers within the system
        /// </summary>
        /// <value></value>
        protected abstract List<string> Layers { get; }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="layer">Name of the current layer</param>
        protected BaseLayer(string layer)
        {
            m_Layer = layer;
        }

        /// <summary>
        /// Checks whether types within this layer may be accessed from types of given other layer.
        /// In a layered architecture a type in a layer may only access types within the layer or
        /// below. In case of strict layering types may only access types on the next lower layer.
        /// </summary>
        /// <param name="otherLayer">The other layer</param>
        /// <param name="strict">true for strict layering</param>
        /// <returns></returns>
        public bool MayBeAccessedFrom(BaseLayer<T> otherLayer, bool strict = false)
        {
            if (!otherLayer.Valid) { return false; }
            if (!Valid) { return false; }

            var currentIndex = Layers.IndexOf(m_Layer);
            var otherIndex = Layers.IndexOf(otherLayer.m_Layer);

            bool result = currentIndex <= otherIndex;
            if (strict)
            {
                result = currentIndex == otherIndex || currentIndex + 1 == otherIndex;
            }

            return result;
        }


        public override bool Equals(object obj)
        {
            return obj is BaseLayer<T> layer && m_Layer == layer.m_Layer;
        }

        public override int GetHashCode()
        {
            return m_Layer.GetHashCode();
        }

        public static bool operator==(BaseLayer<T> first, BaseLayer<T> second) 
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }
            
            if (first is null && !(second is null))
            {
                return false;
            }
            if(!(first is null) && second is null)
            {
                return false;
            }

            if (first is null && second is null)
            {
                return true;
            }

            return first.m_Layer == second.m_Layer;
        }

        public static bool operator!=(BaseLayer<T> first, BaseLayer<T> second) 
        {
            return !(first == second);
        }
    }
}