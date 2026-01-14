using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _VRArchery.Scripts.Runtime.Tutorial
{
    public class TutorialPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tutorialText;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private Button _okButton;
        [SerializeField] private TutorialScrollAnimation _scrollAnimation;
        [SerializeField] private Image _lGripImage;
        [SerializeField] private Image _rTriggerImage;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            _tutorialText.transform.localScale = Vector3.zero;
            _yesButton.transform.localScale = Vector3.zero;
            _noButton.transform.localScale = Vector3.zero;
            _okButton.transform.localScale = Vector3.zero;
            _scrollAnimation.Init();
        }

        /// <summary>
        /// チュートリアルを実行するかどうかをユーザーに問いかけ、その結果を返す
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Yesボタンが押されたらtrue、Noボタンが押されたらfalse</returns>
        public async UniTask<bool> TryTutorialAsync(CancellationToken ct)
        {
            _tutorialText.text = "弓を はなつ 練習を \nはじめるか？";
            _tutorialText.transform.localScale = Vector3.one;

            //巻物を表示させる
            await _scrollAnimation.ShowScrollAnimationAsync(ct);

            await DOTween.Sequence()
                .Append(_yesButton.transform.DOScale(Vector3.one, 0.3f))
                .Append(_noButton.transform.DOScale(Vector3.one, 0.3f))
                .ToUniTask(cancellationToken: ct);

            var result = await UniTask.WhenAny(
                _yesButton.OnClickAsync(ct),
                _noButton.OnClickAsync(ct)
                );


            await DOTween.Sequence()
                .Append(_yesButton.transform.DOScale(Vector3.zero, 0.3f))
                .Append(_noButton.transform.DOScale(Vector3.zero, 0.3f))
                .ToUniTask(cancellationToken: ct);

            // yesボタンなら0(true)を返す
            return result == 0;
        }

        /// <summary>
        /// チュートリアルを開始する
        /// </summary>
        /// <param name="ct"></param>
        public async UniTask StartTutorialAsync(CancellationToken ct)
        {
            _tutorialText.text = "これより 弓の 練習を \nはじめるぞ";
            await _tutorialText.transform.DOScale(Vector3.one, 0.3f).ToUniTask(cancellationToken: ct);

            _okButton.transform.DOScale(Vector3.one, 0.3f)
                .ToUniTask(cancellationToken: ct)
                .Forget();

            await UniTask.WhenAny(_okButton.OnClickAsync(ct), UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: ct));

            await _tutorialText.DOFade(0f, 0f);
            _tutorialText.text = "弓は 左手の \n中指ボタンを \n押して つかむのだ";
            _tutorialText.DOFade(1f, 0.3f);
            _lGripImage.DOFade(1f, 0.3f).ToUniTask(cancellationToken: ct);

            await UniTask.WhenAny(_okButton.OnClickAsync(ct), UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: ct));
            await _tutorialText.DOFade(0f, 0f);
            _tutorialText.text = "右手の 人さし指の ボタンを \nおして 矢を つかみ、\nボタンを はなして \n矢を とばすのだ";
            _lGripImage.DOFade(0f, 0.3f).ToUniTask(cancellationToken: ct);
            _tutorialText.DOFade(1f, 0.3f);
            _rTriggerImage.DOFade(1f, 0.3f).ToUniTask(cancellationToken: ct);

            await UniTask.WhenAny(_okButton.OnClickAsync(ct), UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: ct));
            await _tutorialText.DOFade(0f, 0f);
            _tutorialText.text = "これにて、練習は \nおわりに するぞ";
            _tutorialText.DOFade(1f, 0.3f);
            _rTriggerImage.DOFade(0f, 0.3f).ToUniTask(cancellationToken: ct);

            await UniTask.WhenAny(_okButton.OnClickAsync(ct), UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: ct));
        }

        public async UniTask HideTutorialAsync(CancellationToken ct) => await _scrollAnimation.HideScrollAnimation(ct);
    }
}