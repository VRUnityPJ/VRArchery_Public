
using UnityEngine;

public static class MyMethod
{
    /// <summary>
    /// 親、その親...とComponentを探し最初に見つけたComponentを返す
    /// </summary>
    public static T GetComponentInParents<T>(Transform trans)
    where T : MonoBehaviour
    {
        var parent = trans.parent;
        while (parent != null)
        {
            T result = parent.GetComponent<T>();
            if (result != null)
                return result;

            parent = parent.transform.parent;
        }

        Debug.LogError($"{nameof(T)}が見つかりません");
        return null;
    }
}


