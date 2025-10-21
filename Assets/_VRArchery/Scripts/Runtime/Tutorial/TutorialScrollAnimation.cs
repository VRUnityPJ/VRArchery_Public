using System;
using System.Threading;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace _VRArchery.Scripts.Runtime.Tutorial
{
    public class TutorialScrollAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject _makimonoPrefab;
        [SerializeField] private RectTransform _cloudPrefabUp;
        [SerializeField] private RectTransform _cloudPrefabDown;
        [SerializeField] private RectTransform _paper;
        [SerializeField] private float _cloudMoveValue = 10;
        [SerializeField] private float _cloudShowTime = 1;
        [SerializeField] private float _cloudHideTime = 1;

        private Vector2 _originalCloudScale;
        private Vector3 _originalMakimonoScale;
        private UiAudioPlayer  _audioPlayer;

        private void Start()
        {
            _makimonoPrefab.SetActive(false);
            _originalMakimonoScale = _makimonoPrefab.transform.localScale;
            _audioPlayer = Locator.Resolve<UiAudioPlayer>();
        }

        public void Init()
        {
            _originalCloudScale = _cloudPrefabDown.localScale;

            _cloudPrefabDown.localScale = Vector3.zero;
            _cloudPrefabUp.localScale = Vector3.zero;
        }

        /// <summary>
        /// チュートリアルに表示する巻物のアニメーションを開始する
        /// </summary>
        public async UniTask ShowScrollAnimationAsync(CancellationToken ct)
        {
            _makimonoPrefab.SetActive(true);
            _makimonoPrefab.transform.localScale = Vector3.zero;
            _audioPlayer.PlayScrollStartSound();

            // 巻物のアニメーション
            await DOTween.Sequence()
                .Append(_makimonoPrefab.transform.DOScale(_originalMakimonoScale, 0.1f))
                .Append(_makimonoPrefab.transform.DOMoveX(_makimonoPrefab.transform.position.x + 7, 2f))
                .Join(_paper.transform.DOMoveX(_paper.transform.position.x + 7, 2f))
                .Join(_makimonoPrefab.transform.DORotate(Vector3.up * 360, 2, RotateMode.LocalAxisAdd))
                .ToUniTask(cancellationToken: ct);

            DOTween.Sequence()
                .Append(_cloudPrefabUp.DOScale(_originalCloudScale, _cloudShowTime))
                .Join(_cloudPrefabDown.DOScale(_originalCloudScale, _cloudShowTime))
                .ToUniTask(cancellationToken: ct)
                .Forget();

            DOTween.Sequence()
                .Append(_cloudPrefabDown.DOAnchorPosX(_cloudPrefabDown.anchoredPosition.x + _cloudMoveValue, 3))
                .Append(_cloudPrefabDown.DOAnchorPosX(_cloudPrefabDown.anchoredPosition.x, 3))
                .SetLoops(-1, LoopType.Yoyo)
                .ToUniTask(cancellationToken: ct)
                .Forget();

            DOTween.Sequence()
                .Append(_cloudPrefabUp.DOAnchorPosX(_cloudPrefabUp.anchoredPosition.x - _cloudMoveValue, 3.5f))
                .Append(_cloudPrefabUp.DOAnchorPosX(_cloudPrefabUp.anchoredPosition.x, 3.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .ToUniTask(cancellationToken: ct)
                .Forget();
        }

        /// <summary>
        /// チュートリアルに表示する巻物を非表示にする
        /// </summary>
        public async UniTask HideScrollAnimation(CancellationToken ct)
        {
            await DOTween.Sequence()
                .Append(_cloudPrefabUp.DOScale(Vector3.zero, _cloudHideTime))
                .Join(_cloudPrefabDown.DOScale(Vector3.zero, _cloudHideTime));

            _audioPlayer.PlayScrollEndSound();

            await DOTween.Sequence()
                .Append(_makimonoPrefab.transform.DOMoveX(_makimonoPrefab.transform.position.x - 7, 2f))
                .Join(_paper.transform.DOMoveX(_paper.transform.position.x - 7, 2f))
                .Join(_makimonoPrefab.transform.DORotate(Vector3.up * 360, 2, RotateMode.LocalAxisAdd))
                .Append(_makimonoPrefab.transform.DOScale(Vector3.zero, 0.1f))
                .ToUniTask(cancellationToken: ct);

            _makimonoPrefab.SetActive(false);
        }
    }
}