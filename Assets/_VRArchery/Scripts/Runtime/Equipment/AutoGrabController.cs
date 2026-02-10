using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary> 特定オブフェクトを特定コントローラーに掴ませるスクリプト </summary>
public class AutoGrabController : MonoBehaviour
{
    [Header("Controller Settings")]
    [Tooltip("掴む動作を行う手（Interactor）")]
    [SerializeField]
    private XRBaseInteractor _interactor;

    [Tooltip("自動で掴ませたい対象（Interactable）")]
    [SerializeField] private XRGrabInteractable _targetInteractable;

    private XRInteractionManager _interactionManager;
    private CancellationTokenSource cts = new CancellationTokenSource();

    private void Start()
    {
        // Interactorからマネージャーへの参照を取得
        if (_interactor != null && _interactor.interactionManager != null)
        {
            _interactionManager = _interactor.interactionManager;
        }
        else
        {
            Debug.LogError("XRInteractionManagerが見つかりません。");
        }
    }

    /// <summary>
    /// 強制的にオブジェクトを掴ませる
    /// </summary>
    public async UniTask ForceGrab(CancellationToken token)
    {
        if (CanInteract())
        {
            // すでに何かを掴んでいる場合は離させる
            if (_interactor.hasSelection)
            {
                ForceRelease(cts.Token).Forget();
            }

            // 【修正点】インターフェース型にキャストして渡す
            _interactionManager.SelectEnter(
                (IXRSelectInteractor)_interactor,
                (IXRSelectInteractable)_targetInteractable
            );

            Debug.Log($"Force Grabbed: {_targetInteractable.name}");
        }
    }

    /// <summary>
    /// 強制的にオブジェクトを離させる
    /// </summary>
    public async UniTask ForceRelease(CancellationToken token)
    {
        if (_interactionManager != null && _interactor != null)
        {
            if (_interactor.hasSelection)
            {
                // 現在掴んでいるオブジェクトを取得
                // バージョンによっては interactablesSelected[0] などを使う必要があるため、
                // IXRSelectInteractorインターフェース経由で取得するのが確実です。
                var currentSelect = ((IXRSelectInteractor)_interactor).interactablesSelected;

                if (currentSelect.Count > 0)
                {
                    // 掴んでいる最初のオブジェクトを離す
                    _interactionManager.SelectExit(
                        (IXRSelectInteractor)_interactor,
                        currentSelect[0]
                    );
                    Debug.Log("Force Released");
                }
            }
        }
    }

    private bool CanInteract()
    {
        return _interactionManager != null && _interactor != null && _targetInteractable != null;
    }


}