using UnityEngine;
using UnityEngine.InputSystem;

public class FixingBowWhenTriggered : MonoBehaviour
{
    [SerializeField] private GameObject bow;
    [SerializeField] private InputActionReference leftIndexTriggerAction;

    private Vector3 initialLocalBowPosition;
    private Quaternion initialLocalBowRotation;

    void Start()
    {
        // Bowの初期位置と回転を保存
        initialLocalBowPosition = bow.transform.localPosition;
        initialLocalBowRotation = bow.transform.localRotation;
    }

    void Update()
    {
        // Left Triggerが押されっぱなしの時
        if (leftIndexTriggerAction.action.IsPressed())
        {
            // Bowの位置と回転をその場に固定（Left Controllerとの親子関係を解除）
            bow.transform.parent = null;
        }
        // Left Triggerが押されてない時
        else
        {
            // Bowの位置と回転を初期位置に戻す（Left Controllerとの親子関係を再設定）
            bow.transform.parent = this.transform;
            bow.transform.localPosition = initialLocalBowPosition;
            bow.transform.localRotation = initialLocalBowRotation;
        }
    }
}