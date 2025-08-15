using UnityEngine;
using Zerobject.Laboost.Core.Attributes;

namespace Zerobject.Laboost.Core.Installers
{
    public abstract class BaseScriptableObjectInstaller : ScriptableObject, IInstaller
    {
        public virtual bool Enabled => true;

        [Inject] protected Container Container { get; private set; } = null;

        public abstract void InstallBindings();
    }

    public abstract class ScriptableObjectInstaller : BaseScriptableObjectInstaller
    {
    }
}