using UnityEngine;
using UnityEngine.InputSystem;

public class FixingBowWhenTriggered : MonoBehaviour
{
    [SerializeField] private GameObject _bow;
    [SerializeField] private InputActionReference _leftIndexTriggerAction;

    private Vector3 _initialLocalBowPosition;
    private Quaternion _initialLocalBowRotation;

    void Start()
    {
        // Bowの初期位置と回転を保存
        _initialLocalBowPosition = _bow.transform.localPosition;
        _initialLocalBowRotation = _bow.transform.localRotation;
    }

    void Update()
    {
        // Left Triggerが押されっぱなしの時
        if (_leftIndexTriggerAction.action.IsPressed())
        {
            // Bowがまだ子オブジェクトの場合、親子関係を一度だけ解除して位置を固定
            if (_bow.transform.parent != null)
            {
                _bow.transform.parent = null;
            }

            // 左コントローラーの回転をBowに反映
            _bow.transform.rotation = this.transform.rotation;
        }
        // Left Triggerが押されてない時
        else
        {
            // Bowが親子関係にない場合、関係を再設定する
            if (_bow.transform.parent == null)
            {
                _bow.transform.parent = this.transform;
                _bow.transform.localPosition = _initialLocalBowPosition;
                _bow.transform.localRotation = _initialLocalBowRotation;
            }
        }
    }
}