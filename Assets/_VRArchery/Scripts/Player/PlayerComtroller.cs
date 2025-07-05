using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    private Vector3 _cameraRot;

    // Update is called once per frame
    void Update()
    {
        _cameraRot.x -= Input.GetAxis("Mouse Y");
        _cameraRot.y += Input.GetAxis("Mouse X");

        _camera.transform.rotation = Quaternion.Euler(_cameraRot);
    }
}