using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections; // コルーチンを使用するために必要

public class GrabDetector : MonoBehaviour
{
    private XRGrabInteractable _interactable;

    [SerializeField]
    private ArrowGrabber _arrowGrabber;

    // 処理が重複して実行されるのを防ぐためのフラグ
    private bool _isProcessingGrab = false;

    void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();

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
        var interactor = interactorObject.GetComponent<NearFarInteractor>();

        if (interactor != null)
        {
            // 弓を掴んだ手とは反対の手を判別
            Hand oppositeHand = (interactor.handedness == InteractorHandedness.Left) ? Hand.Right : Hand.Left;
            
            // コルーチンを開始して、1フレーム後に矢を掴ませる処理を呼び出す
            StartCoroutine(GrabArrowWithDelay(oppositeHand));
        }
        else
        {
            Debug.LogWarning("Interactor is not a NearFarInteractor.", interactorObject);
        }
    }

    /// <summary>
    /// 1フレーム待機した後に、矢を掴ませる処理を呼び出すコルーチン
    /// </summary>
    /// <param name="hand">矢を掴ませる手</param>
    private IEnumerator GrabArrowWithDelay(Hand hand)
    {
        _isProcessingGrab = true;

        // 1フレーム待機する。これにより、弓を掴むインタラクションが完全に確定する
        yield return null;

        // 矢を生成して、指定した手に掴ませる
        _arrowGrabber.GrabArrow(hand);
        _arrowGrabber.ArrowGrabHand = hand;

        // 処理が完了したのでフラグを戻す
        _isProcessingGrab = false;
    }

    void OnDestroy()
    {
        if (_interactable != null)
        {
            _interactable.selectEntered.RemoveListener(OnGrabbed);
        }
    }
}