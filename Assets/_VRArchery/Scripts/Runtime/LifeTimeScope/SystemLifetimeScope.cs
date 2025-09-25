using _VRArchery.Scripts.Runtime.Score;
using _VRArchery.Scripts.Runtime.Stage;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _VRArchery.Scripts.Runtime.LifeTimeScope
{
    public class SystemLifetimeScope : LifetimeScope
    {
        [SerializeField] private TargetSpawner _targetSpawner;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponent(_targetSpawner);
            builder.Register<ScoreHolder>(Lifetime.Scoped);
        }
    }
}