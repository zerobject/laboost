using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Zerobject.Laboost.Runtime.Contexts
{
    [DefaultExecutionOrder(-600)]
    [DisallowMultipleComponent, AddComponentMenu("GameObject Context")]
    internal class GameObjectContext : RunnableContext
    {
        [PublicAPI] public event Action PreInstall;
        [PublicAPI] public event Action PostInstall;
        [PublicAPI] public event Action PreResolve;
        [PublicAPI] public event Action PostResolve;

        protected override void SetupContainer()
        {
            SetContainer(new(ProjectContext.Instance?.Container ?? throw new InvalidOperationException(
                "ProjectContext is not initialized. " +
                "Please ensure it is present in the scene before SceneContext.")));
        }

        protected override void Run_Internal()
        {
            PreInstall?.Invoke();
            InstallBindings();
            PostInstall?.Invoke();

            PreResolve?.Invoke();
            ResolveDependencies(GetComponentsInChildren<MonoBehaviour>(true)
               .Where(o => o != this));
            PostResolve?.Invoke();
        }
    }
}