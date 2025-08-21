using System;
using NUnit.Framework;
using Unity.PerformanceTesting;
using Unity.Profiling;
using UnityEngine;
using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Extensions;
using Random = System.Random;

namespace Zerobject.Laboost.Tests.Editor
{
    [TestFixture]
    public class PerformanceTests
    {
        [SetUp]
        public void Setup()
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount  = 0;

            m_RootContainer  = new();
            m_ChildContainer = new(m_RootContainer);

            m_RootContainer.Bind<TestExamples.IFoo>().To<TestExamples.Foo>().FromNew().AsTransient();
            m_RootContainer.Bind<TestExamples.IBar>().To<TestExamples.Bar>().FromNew().AsCached();
            m_RootContainer.Bind<TestExamples.IBaz>().To<TestExamples.Baz>().FromNew().AsSingle();

            m_GcAllocFrame = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");

            for (var i = 0; i < 1000; i++)
            {
                m_RootContainer.Resolve<TestExamples.IFoo>();
                m_RootContainer.Resolve<TestExamples.IBar>();
                m_RootContainer.Resolve<TestExamples.IBaz>();
            }
        }

        [TearDown]
        public void Teardown()
        {
            m_GcAllocFrame.Dispose();
        }

        private Container m_RootContainer;
        private Container m_ChildContainer;

        private ProfilerRecorder m_GcAllocFrame;

        private static readonly Type[] s_ContractTypes =
        {
            typeof(TestExamples.IServiceA),
            typeof(TestExamples.IServiceB),
            typeof(TestExamples.IServiceC),
            typeof(TestExamples.IServiceD),
            typeof(TestExamples.IServiceE)
        };

        private static readonly Random s_Rnd = new();

        [Test]
        [Performance]
        public void Resolve_Transient_100k()
        {
            Measure.Method(() =>
                           {
                               for (var i = 0; i < 100_000; i++)
                                   m_RootContainer.Resolve<TestExamples.IFoo>();
                           })
                   .WarmupCount(5)
                   .MeasurementCount(10)
                   .GC()
                   .Run();
        }

        [Test]
        [Performance]
        public void Resolve_Cached_100k_SameContainer()
        {
            m_RootContainer.Resolve<TestExamples.IBar>();

            Measure.Method(() =>
                           {
                               for (var i = 0; i < 100_000; i++)
                                   m_RootContainer.Resolve<TestExamples.IBar>();
                           })
                   .WarmupCount(5)
                   .MeasurementCount(10)
                   .Run();
        }

        [Test]
        [Performance]
        public void Resolve_Cached_100k_ChildContainer_DoesNotShareCache()
        {
            m_RootContainer.Resolve<TestExamples.IBar>();

            Measure.Method(() =>
                           {
                               for (var i = 0; i < 100_000; i++)
                                   m_ChildContainer.Resolve<TestExamples.IBar>();
                           })
                   .WarmupCount(5)
                   .MeasurementCount(10)
                   .Run();
        }

        [Test]
        [Performance]
        public void Resolve_Single_100k_DeepHierarchy()
        {
            var current = m_RootContainer;
            for (var i = 0; i < 10; i++)
                current = new(current);

            current.Resolve<TestExamples.IBaz>();

            Measure.Method(() =>
                           {
                               for (var i = 0; i < 100_000; i++)
                                   current.Resolve<TestExamples.IBaz>();
                           })
                   .WarmupCount(5)
                   .MeasurementCount(10)
                   .Run();
        }

        [Test]
        [Performance]
        public void Register_10000_Bindings()
        {
            var container = new Container();

            Measure.Method(() =>
                           {
                               for (var i = 0; i < 10_000; i++)
                                   container
                                      .Bind(s_ContractTypes[s_Rnd.Next(s_ContractTypes.Length)])
                                      .To<TestExamples.CompositeService>()
                                      .FromNew()
                                      .AsTransient();
                           })
                   .WarmupCount(1)
                   .MeasurementCount(5)
                   .Run();
        }

        private static class TestExamples
        {
            public interface IFoo
            {
            }

            public class Foo : IFoo
            {
            }

            public interface IBar
            {
            }

            public class Bar : IBar
            {
            }

            public interface IBaz
            {
            }

            public class Baz : IBaz
            {
            }

            public interface IServiceA
            {
            }

            public interface IServiceB
            {
            }

            public interface IServiceC
            {
            }

            public interface IServiceD
            {
            }

            public interface IServiceE
            {
            }

            public class CompositeService : IServiceA, IServiceB, IServiceC, IServiceD, IServiceE
            {
            }
        }

        private static class TypeMarker
        {
            public interface IContract
            {
            }

            public class Impl : IContract
            {
            }
        }
    }
}