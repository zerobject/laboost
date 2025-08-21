using JetBrains.Annotations;
using NUnit.Framework;
using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Exceptions;
using Zerobject.Laboost.Runtime.Extensions;

namespace Zerobject.Laboost.Tests.Runtime
{
    [TestFixture]
    public class ContainerTests
    {
        [SetUp]
        public void Setup() => m_Container = new();

        private Container m_Container;

        [Test]
        public void Container_CorrectBindingRegistration()
        {
            const string methodRelatedClassValue = "wow!";

            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .FromNew()
               .AsSingle();

            m_Container
               .Bind<TestExamples.IMethodRelated>()
               .To<TestExamples.MethodRelatedClass>()
               .FromMethod(() => new TestExamples.MethodRelatedClass(methodRelatedClassValue))
               .AsSingle();

            var instance = new TestExamples.InstanceRelatedClass();
            m_Container
               .BindInstance<TestExamples.IInstanceRelated>(instance)
               .AsSingle();

            Assert.That(m_Container.HasBinding<TestExamples.ICtorRelated>());
            Assert.That(m_Container.HasBinding<TestExamples.IMethodRelated>());
            Assert.That(m_Container.HasBinding<TestExamples.IInstanceRelated>());
        }

        [Test]
        public void Container_SingletonInstancesAreSame()
        {
            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .FromNew()
               .AsSingle();

            var singleton1 = m_Container.Resolve<TestExamples.ICtorRelated>();
            var singleton2 = m_Container.Resolve<TestExamples.ICtorRelated>();

            Assert.AreSame(singleton1, singleton2);
        }

        [Test]
        public void Container_CachedObjectsAreSameAcrossContainer()
        {
            const string cachedClassValue = "cached";

            var parentContainer = new Container();
            var childContainer  = new Container(parentContainer);

            parentContainer
               .Bind<TestExamples.IMethodRelated>()
               .To<TestExamples.MethodRelatedClass>()
               .FromMethod(() => new TestExamples.MethodRelatedClass(cachedClassValue))
               .AsCached();

            var parentInstance1 = parentContainer.Resolve<TestExamples.IMethodRelated>();
            var parentInstance2 = parentContainer.Resolve<TestExamples.IMethodRelated>();

            Assert.AreSame(parentInstance1, parentInstance2);

            var childInstance = childContainer.Resolve<TestExamples.IMethodRelated>();

            Assert.AreNotSame(childInstance, parentInstance1);
        }

        [Test]
        public void Container_TransientObjectsNotSame()
        {
            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .FromNew()
               .AsTransient();

            var transientObject1 = m_Container.Resolve<TestExamples.ICtorRelated>();
            var transientObject2 = m_Container.Resolve<TestExamples.ICtorRelated>();

            Assert.AreNotSame(transientObject1, transientObject2);
        }

        [Test]
        public void BindingBuilder_IgnoreUncorrectBindings()
            => Assert.Throws<BindingTypesMismatchException>(()
                => m_Container
                  .Bind<TestExamples.IFakeContract>()
                  .To<TestExamples.UnrelatedClass>()
                  .FromNew()
                  .AsSingle());

        [Test]
        public void BindAndResolve_WithId_ShouldReturnCorrectInstance()
        {
            const string instance1Id = "instance_1";
            const string instance2Id = "instance_2";

            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .WithId(instance1Id)
               .AsSingle();
            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .WithId(instance2Id)
               .AsSingle();

            Assert.IsInstanceOf<TestExamples.CtorRelatedClass>(
                m_Container.Resolve<TestExamples.ICtorRelated>(instance1Id));
            Assert.IsInstanceOf<TestExamples.CtorRelatedClass>(
                m_Container.Resolve<TestExamples.ICtorRelated>(instance2Id));
        }

        [Test]
        public void BindAndResolve_WithoutId_ShouldReturnDefaultInstance()
        {
            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .FromNew()
               .AsSingle();

            Assert.IsInstanceOf<TestExamples.CtorRelatedClass>(
                m_Container.Resolve<TestExamples.ICtorRelated>());
        }

        [Test]
        public void Resolve_WithInvalidId_ShouldThrowException()
        {
            const string correctId = "correct_id";
            const string invalidId = "invalid_id";

            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .WithId(correctId)
               .AsSingle();

            Assert.Throws<BindingNotFoundException>(()
                => m_Container.Resolve<TestExamples.ICtorRelated>(invalidId));
        }

        [Test]
        public void BindMultipleInstances_WithDifferentIds_ShouldNotConflict()
        {
            const string instance1Id = "instance_1";
            const string instance2Id = "instance_2";

            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .WithId(instance1Id)
               .FromNew()
               .AsSingle();
            m_Container
               .Bind<TestExamples.ICtorRelated>()
               .To<TestExamples.CtorRelatedClass>()
               .WithId(instance2Id)
               .FromNew()
               .AsSingle();

            Assert.AreNotEqual(
                m_Container.Resolve<TestExamples.ICtorRelated>(instance1Id),
                m_Container.Resolve<TestExamples.ICtorRelated>(instance2Id));
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private static class TestExamples
        {
            public interface ICtorRelated { }

            public class CtorRelatedClass : ICtorRelated { }

            public interface IMethodRelated
            {
                string Value { get; }
            }

            public class MethodRelatedClass : IMethodRelated
            {
                public MethodRelatedClass(string value) => Value = value;

                public string Value { get; }
            }

            public interface IInstanceRelated { }

            public class InstanceRelatedClass : IInstanceRelated { }

            public interface IFakeContract { }

            public class UnrelatedClass { }
        }
    }
}