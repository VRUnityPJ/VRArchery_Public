using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public enum Hand
{
    Left,
    Right
}

public class ArrowGrabber : MonoBehaviour
{
    [SerializeField]
    private NearFarInteractor _leftHand;
    [SerializeField]
    private NearFarInteractor _rightHand;
    [SerializeField]
    private GameObject _arrowPrefab;
    /// <summary>
    /// シーン上の弓オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject _bowObject;
    [SerializeField]
    private ArrowCounter _arrowCounter;

    public void GrabArrow(Hand hand)
    {
        if (_arrowPrefab == null)
        {
            Debug.LogError("矢のプレハブが設定されていません。", this);
            return;
        }

        NearFarInteractor interactor = null;
        if (hand == Hand.Left)
        {
            interactor = _leftHand;
        }
        else if (hand == Hand.Right)
        {
            interactor = _rightHand;
        }

        if (interactor == null)
        {
            Debug.LogError("指定されたハンドのインタラクターが設定されていません。", this);
            return;
        }

        if (interactor.hasSelection)
        {
            return;
        }

        GameObject newArrowObj = Instantiate(_arrowPrefab);
        var yaVR = newArrowObj.GetComponent<YaVR>();
        if (yaVR != null)
        {
            yaVR.ArrowGrip = _bowObject;
            yaVR.ArrowCounter = _arrowCounter;
        }
        XRGrabInteractable newInteractable = newArrowObj.GetComponent<XRGrabInteractable>();

        if (newInteractable == null)
        {
            Debug.LogError("生成されたオブジェクトに XRGrabInteractable コンポーネントがありません。", newArrowObj);
            Destroy(newArrowObj);
            return;
        }

        if (interactor.attachTransform != null)
        {
            newArrowObj.transform.position = interactor.attachTransform.position;
            newArrowObj.transform.rotation = interactor.attachTransform.rotation;
        }

        if (interactor is XRBaseInteractor baseInteractor)
        {
            // interactionManagerへのアクセスにはXRBaseInteractorを使用します
            // SelectEnterの引数をIXRSelectInteractorとIXRSelectInteractableにキャストします
            baseInteractor.interactionManager.SelectEnter(baseInteractor as IXRSelectInteractor, newInteractable as IXRSelectInteractable);
        }
    }
}