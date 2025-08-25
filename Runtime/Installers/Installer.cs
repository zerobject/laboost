using Zerobject.Laboost.Runtime.Core;

namespace Zerobject.Laboost.Runtime.Installers
{
    public abstract class InstallerBase : IInstaller
    {
        public Container Container { get; private set; }

        public InstallerType Type => InstallerType.Installer;

        public abstract void InstallBindings();

        protected void SetContainer(Container newContainer)
        {
            Container = newContainer;
        }
    }

    public abstract class Installer : InstallerBase
    {
    }

    public abstract class Installer<TDerived> : InstallerBase
        where TDerived : Installer<TDerived>
    {
    }
}