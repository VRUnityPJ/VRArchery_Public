using UnityEngine;

public class BowVR : MonoBehaviour, IBow
{
    [SerializeField] private GameObject _wirePointObject;
    [SerializeField] private GameObject _wireResetPoint;

    public GameObject GetWirePointObject()
    {
        return _wirePointObject;
    }
    public void ResetWirePointObject()
    {
        _wirePointObject.transform.position = _wireResetPoint.transform.position;
    }
}
