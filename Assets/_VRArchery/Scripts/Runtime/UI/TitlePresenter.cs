using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class TitlePresenter : MonoBehaviour
    {
        /// <summary>
        /// 右側の障子
        /// </summary>
        [SerializeField] private GameObject _rightShoji;

        [SerializeField] private GameObject _outRightShoji;

        /// <summary>
        /// 左側の障子
        /// </summary>
        [SerializeField] private GameObject _leftShoji;

        [SerializeField] private GameObject _outLeftShoji;

        /// <summary>
        /// 障子の移動距離
        /// </summary>
        [SerializeField] private float _moveDistance = 1.8f;

        [SerializeField] private float _moveDuration = 2f;

        /// <summary>
        /// タイトルUI
        /// </summary>
        [SerializeField] private GameObject _titleUI;

        /// <summary>
        /// XR入力アクション
        /// </summary>
        private XRIDefaultInputActions _xrInputAction;

        private void Awake()
        {
            _xrInputAction = new XRIDefaultInputActions();
            _xrInputAction.Enable();
        }

        /// <summary>
        /// 障子を開くアニメーションを再生する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OpenShojiAnimationAsync(CancellationToken token)
        {
             DOTween.Sequence()
                .Append(_rightShoji.transform.DOLocalMoveX(-_moveDistance * 2, _moveDuration))
                .Join(_leftShoji.transform.DOLocalMoveX(_moveDistance * 2, _moveDuration))
                .Join(_outRightShoji.transform.DOLocalMoveX(_moveDistance, _moveDuration))
                .Join(_outLeftShoji.transform.DOLocalMoveX(-_moveDistance, _moveDuration))
                .Join(_titleUI.transform.DOLocalMoveX(-_moveDistance * 5, _moveDuration))
                .SetRelative(true)
                .SetEase(Ease.InOutSine)
                .ToUniTask(cancellationToken: token).Forget();
        }

        /// <summary>
        /// タイトルUIのアニメーションを再生する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask TitleUIAnimationAsync(CancellationToken token)
        {
            await DOTween.Sequence()
                .Append(_titleUI.transform.DOLocalMoveY(0.5f, 2f))
                .SetRelative(true)
                .SetLoops(-1, LoopType.Yoyo)
                .ToUniTask(cancellationToken: token);
        }

        /// <summary>
        /// スタートボタンが押されるまで待機する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnClickStartButtonAsync(CancellationToken token)
        {
            await UniTask.WaitUntil( () =>
            {
                var rightTrigger = _xrInputAction.XRIRightInteraction.ActivateValue.ReadValue<float>();
                return rightTrigger > 0.1f;
            }, cancellationToken: token);
        }
    }
}