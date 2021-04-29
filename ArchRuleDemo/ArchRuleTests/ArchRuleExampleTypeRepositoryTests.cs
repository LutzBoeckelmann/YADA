// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using ArchRuleDemo.ArchitecturalModel;
using NUnit.Framework;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class ArchRuleExampleTypeRepositoryTests
    {
        [Test]
        public void GetTypeRepresentation_AnyImport_NotNull()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("ArchRuleExample.Infrastructure.Module.TechnicalLayer.Non.Architectural.Stuff");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetTypeRepresentation_CorrectInput_ValidResult()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("ArchRuleExample.Infrastructure.Module.Data.Non.Architectural.Stuff");

            Assert.That(result.Valid, Is.True);
        }

        [Test]
        public void GetTypeRepresentation_UnknownPrefix_InValidResult()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("UnknownSystem.Infrastructure.Module.Data.Non.Architectural.Stuff");

            Assert.That(result.Valid, Is.False);
        }


        [Test]
        public void GetTypeRepresentation_CorrectInput_CorrectDomainLayer()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("ArchRuleExample.Infrastructure.Module.TechnicalLayer.Non.Architectural.Stuff");

            Assert.That(result.DomainLayer.Layer, Is.EqualTo("Infrastructure"));
        }

        [Test]
        public void GetTypeRepresentation_UnknownDomainLayer_InValidResult()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("ArchRuleExample.UnknownDomainLayer.Module.Data.Non.Architectural.Stuff");

            Assert.That(result.Valid, Is.False);
            Assert.That(result.DomainLayer.Valid, Is.False);
        }


        [Test]
        public void GetTypeRepresentation_CorrectInput_CorrectTechnicalLayer()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("ArchRuleExample.Infrastructure.Module.Data.Non.Architectural.Stuff");

            Assert.That(result.TechnicalLayer.Layer, Is.EqualTo("Data"));
        }

        [Test]
        public void GetTypeRepresentation_UnknownTechnicalLayer_InValidResult()
        {
            ArchRuleExampleTypeRepository sut = new ArchRuleExampleTypeRepository();
            var result = sut.GetTypeRepresentation("ArchRuleExample.Infrastructure.Module.UnknownTechnicalLayer.Non.Architectural.Stuff");

            Assert.That(result.Valid, Is.False);
            Assert.That(result.TechnicalLayer.Valid, Is.False);
        }

    }
}