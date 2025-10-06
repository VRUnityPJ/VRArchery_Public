using System.Threading;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class GrabDetector : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        [SerializeField]
        private ArrowGrabber _arrowGrabber;

        // 処理が重複して実行されるのを防ぐためのフラグ
        private bool _isProcessingGrab = false;

        private void Awake()
        {
            TryGetComponent<XRGrabInteractable>(out _interactable);

            if (_interactable == null)
            {
                Debug.LogError("XRGrabInteractable component not found on this GameObject.");
                return;
            }

            _interactable.selectEntered.AddListener(OnGrabbed);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            // 既に処理中の場合は、二重に実行しない
            if (_isProcessingGrab) return;

            GameObject interactorObject = args.interactorObject.transform.gameObject;

            // interactorObjectからNearFarInteractorコンポーネントを取得
            interactorObject.TryGetComponent<NearFarInteractor>(out var interactor);

            if (interactor != null)
            {
                // 弓を掴んだ手とは反対の手を判別
                Hand oppositeHand = (interactor.handedness == InteractorHandedness.Left) ? Hand.Right : Hand.Left;

                GrabArrowDelayAsync(oppositeHand, destroyCancellationToken).Forget();
            }
            else
            {
                CustomDebug.LogWarning($"Interactor is not a NearFarInteractor: {interactorObject}");
            }
        }

        /// <summary>
        /// 1フレーム待機した後に、矢を掴ませる処理を呼び出すUniTask
        /// </summary>
        /// <param name="hand">矢を掴ませる手</param>
        private async UniTask GrabArrowDelayAsync(Hand hand, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _isProcessingGrab = true;
            // 1フレーム待機する。これにより、弓を掴むインタラクションが完全に確定する
            await UniTask.NextFrame();
            // 矢を生成して、指定した手に掴ませる
            _arrowGrabber.GrabArrow(hand);
            _arrowGrabber.ArrowGrabHand = hand;

            // 処理が完了したのでフラグを戻す
            _isProcessingGrab = false;
        }

        private void OnDestroy()
        {
            if (_interactable != null)
            {
                _interactable.selectEntered.RemoveListener(OnGrabbed);
            }
        }
    }
}