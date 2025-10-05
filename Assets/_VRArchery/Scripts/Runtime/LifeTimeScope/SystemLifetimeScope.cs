using _VRArchery.Scripts.Runtime.Score;
using _VRArchery.Scripts.Runtime.Stage;
using _VRArchery.Scripts.Runtime.Target;
using _VRArchery.Scripts.Runtime.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _VRArchery.Scripts.Runtime.LifeTimeScope
{
    public class SystemLifetimeScope : LifetimeScope
    {
        [SerializeField] private TargetSpawner _targetSpawner;
        [SerializeField] private ScorePresenter _scorePresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponent(_targetSpawner);
            builder.RegisterComponent(_scorePresenter);
            builder.Register<ScoreHolder>(Lifetime.Scoped);
        }
    }
}