using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Extensions;
using Zerobject.Laboost.Runtime.Installers;
using Debug = UnityEngine.Debug;

namespace Zerobject.Laboost.Runtime.Contexts
{
    public abstract class ContextBase : MonoBehaviour
    {
        [SerializeField] private ContextBase                     m_ParentContext;
        [SerializeField] private List<ScriptableObjectInstaller> m_ScriptableObjectInstallers = new();
        [SerializeField] private List<MonoInstaller>             m_MonoInstallers             = new();
        [SerializeField] private List<MonoInstaller>             m_InstallerPrefabs           = new();

        internal Container Container { get; private set; }

        public IEnumerable<ScriptableObjectInstaller> ScriptableObjectInstallers
        {
            get => m_ScriptableObjectInstallers;
            set
            {
                m_ScriptableObjectInstallers.Clear();
                m_ScriptableObjectInstallers.AddRange(value);
            }
        }

        public IEnumerable<MonoInstaller> MonoInstallers
        {
            get => m_MonoInstallers;
            set
            {
                m_MonoInstallers.Clear();
                m_MonoInstallers.AddRange(value);
            }
        }

        public IEnumerable<MonoInstaller> InstallerPrefabs
        {
            get => m_InstallerPrefabs;
            set
            {
                m_InstallerPrefabs.Clear();
                m_InstallerPrefabs.AddRange(value);
            }
        }

        protected virtual void Awake() => SetupContainer();

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            if (m_ParentContext == this)
            {
                Debug.LogError("Обнаружена циклическая ссылка на родительский контекст.");
                m_ParentContext = null;
            }
        }

        protected abstract void SetupContainer();

        protected void InstallBindings()
        {
            foreach (var monoInstaller in MonoInstallers)
                monoInstaller.SetContainer(Container);
            foreach (var soInstaller in ScriptableObjectInstallers)
                soInstaller.SetContainer(Container);

            foreach (var monoInstaller in MonoInstallers)
                monoInstaller.InstallBindings();
            foreach (var soInstaller in ScriptableObjectInstallers)
                soInstaller.InstallBindings();
        }

        protected void ResolveDependencies(IEnumerable<MonoBehaviour> targets)
        {
            foreach (var target in targets)
                if (target != null && target != this)
                    Container.Inject(target);
        }

        protected void SetContainer(Container container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }
    }
}