// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using Core.DependencyRuleEngine.Rules;
using NUnit.Framework;

namespace YADA.Test 
{
    [TestFixture]
    public class BaseLayerTests
    {
        private class TestLayer : BaseLayer<TestLayer>
        {
            public static TestLayer UpperLayer => new TestLayer("UpperLayer");
            public static TestLayer MiddleLayer => new TestLayer("MiddleLayer");
            public static TestLayer LowerLayer => new TestLayer("LowerLayer");

            protected override List<string> Layers => s_Layers;

            private static readonly List<string> s_Layers = new List<string> { "LowerLayer", "MiddleLayer", "UpperLayer" };
            
            public TestLayer(string layer) : base(layer)
            {
            }
        }


        [Test]
        public void Constructor_KnownLayerName_Valid()
        {
            var sut = new TestLayer("LowerLayer");
            Assert.That(sut.Valid, Is.True);
        }

        [Test]
        public void Constructor_UnKnownLayerName_InValid()
        {
            var sut = new TestLayer("InValidName");
            Assert.That(sut.Valid, Is.False);
        }

        [Test]
        public void MayBeAccessedFrom_StrictLayeringUpperButNotAdjacentLayer_False()
        {
            var sut = TestLayer.LowerLayer;
            var upper = TestLayer.UpperLayer;

            var result = sut.MayBeAccessedFrom(upper, true);

            Assert.That(result, Is.False);
        }

        [Test]
        public void MayBeAccessedFrom_NotStrictLayeringUpperButNotAdjacentLayer_True()
        {
            var sut = TestLayer.LowerLayer;
            var upper = TestLayer.UpperLayer;

            var result = sut.MayBeAccessedFrom(upper);

            Assert.That(result, Is.True);
        }

        [Test]
        public void MayBeAccessedFrom_StrictLayeringUpperAdjacentLayer_True()
        {
            var sut = TestLayer.MiddleLayer;
            var upper = TestLayer.UpperLayer;

            var result = sut.MayBeAccessedFrom(upper, true);

            Assert.That(result, Is.True);
        }

        [Test]
        public void MayBeAccessedFrom_NotStrictLayeringUpperAdjacentLayer_True()
        {
            var sut = TestLayer.LowerLayer;
            var upper = TestLayer.UpperLayer;

            var result = sut.MayBeAccessedFrom(upper);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_TwoDifferentLayers_False()
        {
            var sut = TestLayer.LowerLayer;
            var upper = TestLayer.UpperLayer;

            var result = sut==upper;

            Assert.That(result, Is.False);
        }

        
        [Test]
        public void NotEquals_TwoDifferentLayers_True()
        {
            var sut = TestLayer.LowerLayer;
            var upper = TestLayer.UpperLayer;

            var result = sut!=upper;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_SameLayer_True()
        {
            var sut = TestLayer.LowerLayer;
            var same = sut;
            
            var result = sut==same;

            Assert.That(result, Is.True);
        }

        
        [Test]
        public void NotEquals_SameLayer_False()
        {
            var sut = TestLayer.LowerLayer;
            var same = sut;
            
            var result = sut!=same;

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Null_False()
        {
            var sut = TestLayer.LowerLayer;

            var result = sut==null && null == sut;

            Assert.That(result, Is.False);
        }

        
        [Test]
        public void NotEquals_Null_True()
        {
            var sut = TestLayer.LowerLayer;

            var result = sut != null;
            result &= null != sut;
            Assert.That(result, Is.True);
        }

    }
}