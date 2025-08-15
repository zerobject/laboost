namespace Zerobject.Laboost.Core.Installers
{
    public interface IInstaller
    {
        bool Enabled { get; }
        void InstallBindings();
    }
}