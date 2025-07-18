using _VRArchery.Scripts.Runtime.Score;
using NUnit.Framework;

namespace _VRArchery.Scripts.Tests.Editor
{
    public class ScoreTests
    {
        private ScoreHolder _score;

        [Test, Order(1)]
        public void 初期化テスト()
        {
            _score = new ScoreHolder();
            _score.InitializeScore();
            Assert.That(0 == _score.Score.CurrentValue);
        }

        [Test]
        public void 正常に加算されるか()
        {
            _score.AddScore(100);
            Assert.That(100 == _score.Score.CurrentValue);
            _score.AddScore(200);
            Assert.That(300 == _score.Score.CurrentValue);
        }

        [Test]
        public void 正常に減算され0以下にならないか()
        {
            _score.SubScore(100);
            Assert.That(200 == _score.Score.CurrentValue);
            _score.SubScore(9999);
            Assert.That(0 == _score.Score.CurrentValue);
        }
    }
}