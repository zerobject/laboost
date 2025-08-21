using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using FindObjSortMode = UnityEngine.FindObjectsSortMode;
using FindObjInactive = UnityEngine.FindObjectsInactive;

namespace Zerobject.Laboost.Runtime.Contexts
{
    [DefaultExecutionOrder(-800)]
    [DisallowMultipleComponent, AddComponentMenu("Scene Context")]
    internal class SceneContext : RunnableContext
    {
        [PublicAPI] public event Action PreInstall;
        [PublicAPI] public event Action PostInstall;
        [PublicAPI] public event Action PreResolve;
        [PublicAPI] public event Action PostResolve;

        public UnityEvent OnPreInstall;
        public UnityEvent OnPostInstall;
        public UnityEvent OnPreResolve;
        public UnityEvent OnPostResolve;

        protected override void SetupContainer()
        {
            SetContainer(new(ProjectContext.Instance?.Container ?? throw new InvalidOperationException(
                "ProjectContext is not initialized. " +
                "Please ensure it is present in the scene before SceneContext.")));
        }

        protected override void Run_Internal()
        {
            PreInstall?.Invoke();
            OnPreInstall?.Invoke();

            InstallBindings();

            PostInstall?.Invoke();
            OnPostInstall?.Invoke();

            PreResolve?.Invoke();
            OnPreResolve?.Invoke();

            ResolveDependencies(FindObjectsByType<MonoBehaviour>(FindObjInactive.Include, FindObjSortMode.None)
               .Where(o => o != null && o != this && o.gameObject.scene == gameObject.scene));

            PostResolve?.Invoke();
            OnPostResolve?.Invoke();
        }
    }
}