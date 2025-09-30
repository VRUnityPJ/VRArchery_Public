using System;
using UnityEngine;
using UnityEngine.EventSystems;

//参考　https://light11.hatenadiary.com/entry/2018/01/31/234716
/// <summary>
/// VRゲームに使用するボタンの基底クラス
/// </summary>
public class VRButton : MonoBehaviour
{
    private EventTrigger _trigger;

    protected virtual void Awake()
    {
        if(!TryGetComponent(out _trigger))
            Debug.LogError("Triggerが取得できていません");
    }

    /// <summary>
    /// クリックされたときのイベントを追加する
    /// </summary>
    public void AddOnBeginClickListener(Action<PointerEventData> callback)
    {
        AddEventListener(EventTriggerType.PointerDown,callback);
    }
    /// <summary>
    /// カーソルがボタンにかぶさった時のイベントを追加する
    /// </summary>
    public void AddOnTouchListener(Action<PointerEventData> callback)
    {
        AddEventListener(EventTriggerType.PointerEnter,callback);
    }
    /// <summary>
    /// カーソルがボタンから離れたときのイベントを追加する
    /// </summary>
    public void AddOnExitListener(Action<PointerEventData> callback)
    {
        AddEventListener(EventTriggerType.PointerExit,callback);
    }
    /// <summary>
    /// クリックし終わった時のイベントを追加する
    /// </summary>
    public void AddOnEndClickListener(Action<PointerEventData> callback)
    {
        AddEventListener(EventTriggerType.PointerUp,callback);
    }
    /// <summary>
    /// 指定されたトリガーにイベントを追加する関数
    /// </summary>
    private void AddEventListener<T>(EventTriggerType triggerType, Action<T> callback)
        where T : PointerEventData
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(data => callback(data as T));
        _trigger.triggers.Add(entry);
    }
}