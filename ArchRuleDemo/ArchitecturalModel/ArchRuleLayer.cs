// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace ArchRuleDemo.ArchitecturalModel
{
    public abstract class ArchRuleLayer<T> where T : ArchRuleLayer<T>
    {
        public bool Valid { get; }
        public string Layer { get; }

        protected List<string> Layers { get; }

        protected ArchRuleLayer(string layer, List<string> suitableLayer)
        {
            Layers = suitableLayer;
            if (Layers.Contains(layer))
            {
                Valid = true;
                Layer = layer;
            }
        }

        public bool MayBeAccessedFrom(T otherLayer, bool strict = false)
        {
            if (!otherLayer.Valid) { return false; }
            if (!Valid) { return false; }

            var currentIndex = Layers.IndexOf(Layer);
            var otherIndex = Layers.IndexOf(otherLayer.Layer);


            bool result = currentIndex <= otherIndex;
            if (strict)
            {
                result = currentIndex == otherIndex || currentIndex + 1 == otherIndex;
            }

            return result;
        }
    }
}