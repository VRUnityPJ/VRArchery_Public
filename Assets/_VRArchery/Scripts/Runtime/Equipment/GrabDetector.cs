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

        void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();

            if (_interactable == null)
            {
                Debug.LogError("XRGrabInteractable component not found on this GameObject.");
                return;
            }

            // 掴まれた時のイベントにメソッドを登録
            _interactable.selectEntered.AddListener(OnGrabbed);
        }

        /// <summary>
        /// オブジェクトが掴まれた時に呼び出されるメソッド
        /// </summary>
        /// <param name="args">イベントの引数。掴んだInteractorの情報が含まれる。</param>
        private void OnGrabbed(SelectEnterEventArgs args)
        {
            // 掴んだInteractorのGameObjectを取得
            GameObject interactorObject = args.interactorObject.transform.gameObject;

            Debug.Log($"オブジェクト '{gameObject.name}' が掴まれました！");
            Debug.Log($"掴んだのは '{interactorObject.name}' です。");

            if (interactorObject.name.Contains("Near-Far"))
            {
                var interactor = interactorObject.GetComponent<NearFarInteractor>();
                if (interactor.handedness == InteractorHandedness.Left)
                {
                    Debug.Log("左手で掴まれました。");
                    _arrowGrabber.GrabArrow(Hand.Right);
                    _arrowGrabber.ArrowGrabHand = Hand.Right;

                }
                else if (interactor.handedness == InteractorHandedness.Right)
                {
                    Debug.Log("右手で掴まれました。");
                    _arrowGrabber.GrabArrow(Hand.Left);
                    _arrowGrabber.ArrowGrabHand = Hand.Left;
                }
                else
                {
                    Debug.Log("不明な方の手によって掴まれました。");
                }
            }
            else
            {
                Debug.Log("不明なInteractorによって掴まれました。");
            }

            // ここに、掴んだInteractorに応じた処理を記述
            // 例: 特定の手で掴んだ時だけ特殊なエフェクトを再生する
        }

        void OnDestroy()
        {
            if (_interactable != null)
            {
                _interactable.selectEntered.RemoveListener(OnGrabbed);
            }
        }
    }
}