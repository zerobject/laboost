using Zerobject.Laboost.Core.Attributes;

namespace Zerobject.Laboost.Core.Installers
{
    public abstract class BaseInstaller : IInstaller
    {
        [Inject] protected Container Container { get; set; }

        public virtual bool Enabled => true;

        public abstract void InstallBindings();
    }

    public abstract class Installer : BaseInstaller
    {
    }

    public abstract class Installer<TDerived> : BaseInstaller where TDerived : Installer<TDerived>
    {
        public static void Install(Container container)
        {
        }
    }
}