using UnityEngine;

namespace Zerobject.Laboost.Core.Installers
{
    using Attributes;

    public abstract class BaseMonoInstaller : MonoBehaviour, IInstaller
    {
        public virtual bool Enabled => true;

        [Inject] protected Container Container { get; set; }

        public abstract void InstallBindings();
    }

    public abstract class MonoInstaller : BaseMonoInstaller
    {
    }
}