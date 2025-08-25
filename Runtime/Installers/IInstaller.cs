namespace Zerobject.Laboost.Runtime.Installers
{
    public enum InstallerType { Installer, MonoInstaller, ScriptableObjectInstaller }

    public interface IInstaller
    {
        InstallerType Type { get; }
        void InstallBindings();
    }
}