using UnityEngine;

public class YaFlyingManager : MonoBehaviour
{
    internal bool IsFlying = false;

    private Vector3 _prePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _prePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFlying)//進行方向に回転
        {
            Vector3 velocity = transform.position - _prePosition;
            if(velocity.magnitude > 0.01f)
            {
                Debug.Log("flying away");
                transform.rotation = Quaternion.LookRotation(velocity);
            }
            _prePosition = transform.position;
        }
    }
}
