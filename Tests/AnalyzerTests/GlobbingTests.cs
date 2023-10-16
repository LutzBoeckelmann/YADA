// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.Analyzer;

namespace YADA.AnalyzerTests
{
    [TestFixture]
    public class GlobbingTests
    {
        [Test]
        public void IsMatch_InputEqualsPatter_True()
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("Test");
            var result = sut.IsMatch("Test");
            Assert.IsTrue(result);
        }

        [Test]
        public void IsMatch_InputContainsPattern_True()
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("Test");
            var result = sut.IsMatch("SomeImputTestSomethingMore");
            Assert.IsTrue(result);
        }
        [Test]
        public void IsMatch_UnMatchingInput_False()
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("Test");
            var result = sut.IsMatch("SomeImputWithoutAnyPatternSomethingMore");
            Assert.IsFalse(result);
        }

        [Test]
        public void IsMatch_StarMatchesAnything()
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("*");
            var result = sut.IsMatch("SomeImputWithPatternSomethingMore");
            Assert.IsTrue(result);
        }

        [TestCase("SomeImputWithPatternWithDotSomethingMore", true)]
        [TestCase("SomeImputWithPatternGWithDotthingMore", false)]
        public void IsMatch_StarExpanseUntilBlocked(string input, bool expected)
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("Pa*Some");
            var result = sut.IsMatch(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("SomeImputWithPattern.WithDotSomethingMore", true)]
        [TestCase("SomeImputWithPatternGWithDotSomethingMore", false)]
        public void IsMatch_DotIsMatchedOnlyAsDot(string input, bool expected) 
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("Pattern.WithDot");
            var result = sut.IsMatch(input);
            Assert.That(result, Is.EqualTo(expected));
        }


        [TestCase("SomeImput.WithPatternSomething.More", true)]
        [TestCase("SomeImput.WithPattern.Something.More", false)]
        public void IsMatch_DotStopsStar(string input, bool expected)
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("With*Some");
            var result = sut.IsMatch(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("SomeImput.WithPattern.Something.More", true)]
        [TestCase("SomeImput.With.Pattern.Something.More", true)]
        [TestCase("SomeImput.With...Pa ttern.Something.More", true)]
        public void IsMatch_DoubleStarIsNotBlockedByDots(string input, bool expected)
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("With**Some");
            var result = sut.IsMatch(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("Example.Example1", true)]
        public void IsMatch_DoubleStarIsNotBlockedByDots1(string input, bool expected)
        {
            GlobPatternMatcher sut = new GlobPatternMatcher("Example.Example1");
            var result = sut.IsMatch(input);
            Assert.That(result, Is.EqualTo(expected));
        }

    }
}
