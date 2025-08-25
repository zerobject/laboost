using UnityEngine;
using Zerobject.Laboost.Runtime.Core;

namespace Zerobject.Laboost.Runtime.Installers
{
    public abstract class MonoInstallerBase : MonoBehaviour, IInstaller
    {
        public Container Container { get; private set; }

        public InstallerType Type => InstallerType.MonoInstaller;

        public abstract void InstallBindings();

        internal void SetContainer(Container newContainer)
        {
            Container = newContainer;
        }
    }

    public abstract class MonoInstaller : MonoInstallerBase
    {
    }

    public abstract class MonoInstaller<TDerived>
        : MonoInstallerBase where TDerived : MonoInstaller<TDerived>
    {
    }
}