using UnityEngine;
using UnityEngine.UIElements;

public class Ya : MonoBehaviour
{
    public bool _isFlying;
    private Vector3 _prePosition;
    private Rigidbody _rb;
    private BoxCollider _boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isFlying)//進行方向に回転
        {
            Vector3 velocity = transform.position - _prePosition;
            if(velocity.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(velocity);
            }
            _prePosition = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            GameManager.instance.SetTargetModleMarker(collision.GetContact(0).point - collision.transform.position);
        }
        else
        {
            GameManager.instance.SetTargetModleMarker(Vector3.one * -1);
        }
        _isFlying = false;
        _rb.isKinematic = true;
        _boxCollider.isTrigger = true;
    }

}