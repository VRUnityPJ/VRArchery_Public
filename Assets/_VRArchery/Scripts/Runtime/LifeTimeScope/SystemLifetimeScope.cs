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
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private MainSequence _mainSequence;
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponent(_mainSequence);
            builder.RegisterComponent(_targetSpawner);
            builder.RegisterComponent(_scorePresenter);
            builder.RegisterComponent(_playerTransform);

            builder.Register<ScoreHolder>(Lifetime.Scoped);
        }
    }
}