using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zerobject.Laboost.Runtime.Contexts;

namespace Zerobject.Laboost.Tests.Runtime
{
    [TestFixture]
    public class ProjectContextTests
    {
        [UnityTest]
        public IEnumerator ProjectContextCreationTest()
        {
            GameObject testContextObj = new(nameof(testContextObj));
            var        testContext    = testContextObj.AddComponent<ProjectContext>();

            yield return null;

            Assert.NotNull(ProjectContext.Instance);
            Assert.AreEqual(testContext, ProjectContext.Instance);

            Object.DestroyImmediate(testContextObj);
        }

        [UnityTest]
        public IEnumerator ProjectContext_InitMethod_SpawnsPrefabOrDefault()
        {
            ProjectContext.Instance = null;
            ProjectContext.TestInit();

            yield return null;

            Assert.NotNull(ProjectContext.Instance);
            Object.DestroyImmediate(ProjectContext.Instance.gameObject);
        }
    }
}