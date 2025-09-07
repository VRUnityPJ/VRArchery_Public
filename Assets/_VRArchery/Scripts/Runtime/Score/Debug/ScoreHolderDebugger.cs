using UnityEngine;
using _VRArchery.Scripts.Runtime.Score;

namespace _VRArchery.Scripts.Runtime.Score.Debugging
{
    public sealed class ScoreHolderDebugger : MonoBehaviour
    {
        [SerializeField] private int _score = 0;

        private int _lastScore;
        private ScoreHolder _scoreHolder;

        private void Awake()
        {
            _scoreHolder = new ScoreHolder();
            _scoreHolder.InitializeScore();
            _lastScore = _score;
        }

        private void Update()
        {
            if (_score != _lastScore)
            {
                _scoreHolder.InitializeScore();
                if (_score > 0) _scoreHolder.AddScore(_score);
                LogScoreAndRank();
                _lastScore = _score;
            }
        }

        private void LogScoreAndRank()
        {
            var score = _scoreHolder.Score.CurrentValue;
            var rank = _scoreHolder.GetRank().ToString();
            Debug.Log($"スコア：{score}\nランク：{rank}");
        }
    }
}
