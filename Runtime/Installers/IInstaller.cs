namespace Zerobject.Laboost.Runtime.Installers
{
    public enum InstallerType { MonoInstaller, ScriptableObjectInstaller }

    public interface IInstaller
    {
        void InstallBindings();
    }
}