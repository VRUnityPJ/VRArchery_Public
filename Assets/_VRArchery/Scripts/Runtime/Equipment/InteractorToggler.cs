using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Input System必須
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary> 指定したInputActionの操作の有効化/無効化を切り替えるスクリプト </summary>
public class InteractorToggler : MonoBehaviour
{
    [Header("Input Actions to Block")]
    [Tooltip("無効化したい入力アクション（Selectなど）をここに登録してください")]
    [SerializeField] private List<InputActionReference> _targetActions = new List<InputActionReference>();

    [Header("Safety Settings")]
    [Tooltip("何かを掴んでいる間は、無効化を禁止するか")]
    [SerializeField] private bool _preventDisableWhileGrabbing = true;

    [Tooltip("掴んでいるかチェックするためのInteractor（任意）")]
    [SerializeField] private List<XRBaseInteractor> _interactorsToCheck = new List<XRBaseInteractor>();

    // 現在の状態
    public bool IsInputEnabled { get; private set; } = true;

    private void Start()
    {
        // Interactorが空なら、このオブジェクトや子から自動で探しておく（安全策）
        if (_interactorsToCheck.Count == 0)
        {
            var found = GetComponentsInChildren<XRBaseInteractor>();
            _interactorsToCheck.AddRange(found);
        }
    }

    /// <summary>
    /// 入力の有効/無効を切り替える
    /// </summary>
    public void ToggleInput()
    {
        SetInput(!IsInputEnabled);
    }

    /// <summary>
    /// 明示的に入力の有効/無効を指定する
    /// </summary>
    public void SetInput(bool enable)
    {
        // OFFにしようとした時、物を掴んでいたら中止するチェック
        if (!enable && IsInputEnabled && _preventDisableWhileGrabbing)
        {
            if (IsGrabbingAny())
            {
                Debug.Log("Cannot disable input while grabbing an object.");
                return;
            }
        }

        IsInputEnabled = enable;
        ApplyState();
    }

    private void ApplyState()
    {
        foreach (var actionRef in _targetActions)
        {
            if (actionRef != null && actionRef.action != null)
            {
                if (IsInputEnabled)
                {
                    // 入力を有効化（ボタンが効くようになる）
                    actionRef.action.Enable();
                }
                else
                {
                    // 入力を無効化（ボタンが効かなくなる）
                    actionRef.action.Disable();
                }
            }
        }
        Debug.Log($"Input Actions: {(IsInputEnabled ? "Enabled" : "Disabled")}");
    }

    private bool IsGrabbingAny()
    {
        foreach (var interactor in _interactorsToCheck)
        {
            if (interactor != null && interactor.hasSelection)
            {
                return true;
            }
        }
        return false;
    }

    // アプリケーション終了時などに、入力が無効のままだと困るので戻しておく
    private void OnDisable()
    {
        foreach (var actionRef in _targetActions)
        {
            if (actionRef != null && actionRef.action != null)
            {
                actionRef.action.Enable();
            }
        }
    }
}