using UnityEngine;
using Zerobject.Laboost.Runtime.Core;

namespace Zerobject.Laboost.Runtime.Installers
{
    public abstract class ScriptableObjectInstallerBase : ScriptableObject, IInstaller
    {
        public Container Container { get; private set; }

        public abstract void InstallBindings();

        internal void SetContainer(Container newContainer)
        {
            Container = newContainer;
        }
    }

    public abstract class ScriptableObjectInstaller : ScriptableObjectInstallerBase
    {
    }

    public abstract class ScriptableObjectInstaller<TDerived> 
        : ScriptableObjectInstallerBase where TDerived : ScriptableObjectInstaller<TDerived>
    {
    }
}