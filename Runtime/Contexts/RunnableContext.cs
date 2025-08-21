using JetBrains.Annotations;
using UnityEngine;

namespace Zerobject.Laboost.Runtime.Contexts
{
    public abstract class RunnableContext : ContextBase
    {
        [SerializeField] protected bool IsRunning;
        [SerializeField] protected bool AutoRun;

        protected override void Awake()
        {
            base.Awake();
            
            if (AutoRun)
                Run();
        }
        
        [PublicAPI]
        public void Run()
        {
            if (IsRunning) return;
            IsRunning = true;
            
            Run_Internal();
        }

        protected abstract void Run_Internal();
    }
}