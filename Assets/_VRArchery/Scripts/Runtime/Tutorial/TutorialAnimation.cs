using System;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace _VRArchery.Scripts.Runtime.Tutorial
{
    public class TutorialAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject _makimonoPrefab;
        [SerializeField] private RectTransform _cloudPrefabUp;
        [SerializeField] private RectTransform _cloudPrefabDown;
        [SerializeField] private RectTransform _paper;

        private async UniTaskVoid Start()
        {
            DOTween.Sequence()
                .Append(_makimonoPrefab.transform.DOMoveX(_makimonoPrefab.transform.position.x + 7, 2f))
                .Join(_paper.transform.DOMoveX(_paper.transform.position.x + 7, 2f))
                .Join(_makimonoPrefab.transform.DORotate(Vector3.up * 360, 2, RotateMode.LocalAxisAdd))
                .ToUniTask(cancellationToken: destroyCancellationToken)
                .Forget();


            DOTween.Sequence()
                .Append(_cloudPrefabDown.DOAnchorPosX(_cloudPrefabDown.anchoredPosition.x + 0.5f, 3))
                .Append(_cloudPrefabDown.DOAnchorPosX(_cloudPrefabDown.anchoredPosition.x, 3))
                .SetLoops(-1, LoopType.Yoyo)
                .ToUniTask(cancellationToken: destroyCancellationToken)
                .Forget();

            DOTween.Sequence()
                .Append(_cloudPrefabUp.DOAnchorPosX(_cloudPrefabUp.anchoredPosition.x - 0.5f, 3.5f))
                .Append(_cloudPrefabUp.DOAnchorPosX(_cloudPrefabUp.anchoredPosition.x, 3.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .ToUniTask(cancellationToken: destroyCancellationToken)
                .Forget();
        }
    }
}