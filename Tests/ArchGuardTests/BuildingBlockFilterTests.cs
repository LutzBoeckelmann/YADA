using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using YADA.Analyzer;
using YADA.ArchGuard.BuildingBlock;

namespace ArchGuardTests
{
    [TestFixture]
    public class BuildingBlockFilterTests
    {
        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test(string types)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1.*", "and", "not", ".*Core\\.Sub1\\.Impl.*" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(types, "Blub")), Is.EqualTo(Test1Coded(types)));
        }

        private bool Test1Coded(string input)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return terminal1.IsMatch(input) && !terminal2.IsMatch(input);
        }

        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test2(string type)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1.*", "and", "not", ".*Core\\.Sub1\\.Impl.*", "or", ".*Core\\.Sub1\\.Impl.*" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test2Coded(type)));
        }

        private bool Test2Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return terminal1.IsMatch(type) && !terminal2.IsMatch(type) || terminal2.IsMatch(type);
        }


        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test3(string type)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1.*", "and", "not", ".*Core\\.Sub1\\.Impl.*", "and", ".*Core\\.Sub1\\.Impl.*" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test3Coded(type)));
        }

        private bool Test3Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return terminal1.IsMatch(type) && !terminal2.IsMatch(type) && terminal2.IsMatch(type);
        }




        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test4(string type)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1.*", "or", "not", ".*Core\\.Sub1\\.Impl.*", "and", ".*Core\\.Sub1\\.Impl.*" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);


            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test4Coded(type)));
        }

        private bool Test4Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");
            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            Func<string, bool> func1 = s => { Console.WriteLine("Func1"); return terminal1.IsMatch(s); };
            Func<string, bool> func2 = s => { Console.WriteLine("Func2"); return terminal2.IsMatch(s); };
            Func<string, bool> func3 = s => { Console.WriteLine("Func3"); return terminal2.IsMatch(s); };

            return func1(type) | !func2(type) & func3(type);
        }



        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test5(string type)
        {
            List<string> input = new List<string> { "(", ".*Core\\.Sub1.*", "or", ".*Core\\.Sub1\\.Impl.*", ")" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test5Coded(type)));
        }

        private bool Test5Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return (terminal1.IsMatch(type) || terminal2.IsMatch(type));
        }


        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test6(string type)
        {
            List<string> input = new List<string> { "(", ".*Core\\.Sub1.*", "or", "not", ".*Core\\.Sub1\\.Impl.*", ")", "and", ".*Core\\.Sub1\\.Impl.*" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test6Coded(type)));
        }

        private bool Test6Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return (terminal1.IsMatch(type) || !terminal2.IsMatch(type)) && terminal2.IsMatch(type);
        }

        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test7(string type)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1\\.Impl.*", "and", "(", ".*Core\\.Sub1.*", "or", "not", ".*Core\\.Sub1\\.Impl.*", ")" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test7Coded(type)));
        }

        private bool Test7Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return terminal2.IsMatch(type) && (terminal1.IsMatch(type) || !terminal2.IsMatch(type));
        }

        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test8(string type)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1\\.Impl.*", "and", "(", ".*Core\\.Sub1.*", "and", "not", "(", ".*Core\\.Sub1.*", "or", "not", ".*Core\\.Sub1\\.Impl.*", ")", ")" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test8Coded(type)));
        }

        private bool Test8Coded(string type)
        {
            var terminal1 = new Regex(".*Core\\.Sub1.*");

            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return terminal2.IsMatch(type) && (terminal1.IsMatch(type) && !(terminal1.IsMatch(type) || !terminal2.IsMatch(type)));
        }


        [TestCase("MyProject.Core.Sub1.IClass1")]
        [TestCase("MyProject.Core.Sub1.Impl.Class1")]
        public void Test9(string type)
        {
            List<string> input = new List<string> { ".*Core\\.Sub1\\.Impl.*" };

            IBuildingBlockTypeFilter sut = new BuildingBlockTypeFilter(input);

            Assert.That(sut.Match(new TypeDescriptionFake(type, "Blub")), Is.EqualTo(Test9Coded(type)));
        }

        private bool Test9Coded(string type)
        {
            var terminal2 = new Regex(".*Core\\.Sub1\\.Impl.*");

            return terminal2.IsMatch(type);
        }
    }
}
