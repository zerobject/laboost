using System.Diagnostics;
using UnityEngine;

namespace Zerobject.Laboost.Runtime.Contexts
{
    [DefaultExecutionOrder(-1000)]
    [DisallowMultipleComponent, AddComponentMenu("Project Context")]
    internal class ProjectContext : ContextBase
    {
        public static ProjectContext Instance { get; internal set; }

        protected override void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            base.Awake();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if (Instance != null) return;

            var prefab = Resources.Load<GameObject>(nameof(ProjectContext));
            var clone  = prefab ? Instantiate(prefab) : new(nameof(ProjectContext), typeof(ProjectContext));
            clone.name = nameof(ProjectContext);
        }

        protected override void SetupContainer()
        {
            SetContainer(new());
        }

        [Conditional("UNITY_EDITOR")]
        internal static void TestInit() => Init();
    }
}