using R3;
using Ranking.Demo.Scripts.DemoGame;
using UnityEngine;

namespace RankingSystem.Scripts
{
    public class ScoreUIPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerScoreHolder _model;
        [SerializeField] private ScoreUIViewer _viewer;
        private void Start()
        {
            _model.Score.Subscribe(val =>
            {
                _viewer.UpdateText(val.IntValue);
            }).AddTo(this);
        }
    }
}