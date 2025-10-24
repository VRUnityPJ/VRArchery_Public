using RankingSystem.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KeyBoard
{
    public class KeyBoardLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private EnterButton _enterButton;
        [SerializeField]
        private InputKeyCollector _inputcol;
        [SerializeField]
        private RankingStorage _rankingStorage;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(_enterButton);
            builder.RegisterInstance(_inputcol);
            builder.RegisterInstance(_rankingStorage);
            builder.RegisterEntryPoint<EnterController>().As<ICompletable>();

        }
    }
}