using _VRArchery.Scripts.Runtime.Score;
using VContainer;
using VContainer.Unity;

namespace RankingSystem.Scripts
{
    /// <summary>
    /// スコアとランキングシステムを中継するクラス
    /// </summary>
    public class RankingScoreAdaptor
    {
        private ScoreHolder _scoreHolder;
        private RankingStorage _rankingStorage;

        public RankingScoreAdaptor(ScoreHolder scoreHolder)
        {
            _scoreHolder = scoreHolder;
        }

        /// <summary>
        /// ランキングを登録する
        /// </summary>
        public void Register()
        {
            _rankingStorage = RankingStorage.instance;
            _rankingStorage.UpdateData(_scoreHolder);
            _rankingStorage.Register();
        }
    }
}