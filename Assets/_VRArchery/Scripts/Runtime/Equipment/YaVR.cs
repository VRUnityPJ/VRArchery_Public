using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class YaVR : MonoBehaviour
{
    private bool _isFlying ;
    private bool _isNocking = false;
    private bool _canNock = false;
    private Vector3 _prePosition;
    private Rigidbody _rb;
    private BoxCollider _boxCollider;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _speed = 40;
    /// <summary>
    /// 矢をつがえる位置の当たり判定
    /// </summary>
    [SerializeField] private GameObject ArrowGrip;
    [SerializeField] YaFlyingManager yaFlyingManager;
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private XRGrabInteractable _grabInteract;
    private InputAction nockAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var actionMap = _actionAsset.FindActionMap("Archery");
        if (actionMap != null)
        {
            nockAction = actionMap.FindAction("nockArrow", true);
            Debug.Log("nock arrow attached");
            if (nockAction == null)
            {
                Debug.LogError("Cannot find nock arrow");
            }
        }
        else
        {
            Debug.LogError("No action asset found");
        }
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _grabInteract.trackRotation = true;
    }

    void OnEnable()
    {
        if (nockAction == null) return;
        nockAction.Enable();

        nockAction.performed += OnTriggerRightPressed;
        nockAction.canceled += OnTriggerRightReleased;
        Debug.Log("nockAction enabled");
    }

    // Update is called once per frame
    void Update()
    {
        /*if(_isFlying)//進行方向に回転
        {
            Debug.Log("flying away");
            Vector3 velocity = transform.position - _prePosition;
            if(velocity.magnitude > 0.01f)
            {
                Debug.Log("flying away");
                transform.rotation = Quaternion.LookRotation(velocity);
            }
            _prePosition = transform.position;
        }*/

        if (_isNocking)
        {
            Vector3 direction = ArrowGrip.transform.position - this.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rb.rotation = targetRotation;
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
        _rb.useGravity = false;
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Nock"))
        {
            Debug.Log(" Can Nock");
            _canNock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Nock"))
        {
            Debug.Log("Cannot Nock");
            _canNock = false;
            _grabInteract.trackRotation = true;
        }
    }

    private void OnTriggerRightPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnTriggerRightPressed");
        if (_canNock)
        {
            Debug.Log("An Arrow Is Nocking");
            _isNocking = true;
            _grabInteract.trackRotation = false;
            _rb.useGravity = false;
            _isFlying = false;
        }
    }

    private void OnTriggerRightReleased(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnTriggerRightReleased");
        if (_isNocking)
        {
            Debug.Log("An Arrow is Shooted");
            _isNocking = false;
            ForceRelease();
            yaFlyingManager.IsFlying = true;
            _rb.useGravity = true;
            if (ArrowGrip.transform.position != null)
            {
                Debug.Log($"Vector: {ArrowGrip.transform.position- this.transform.position}");
                _rb.AddForce((ArrowGrip.transform.position - this.transform.position) * _speed, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("No Arrow Grip");
            }

        }
    }

    private void ForceRelease()
    {
        var interactor = _grabInteract.interactorsSelecting.FirstOrDefault();
        if (interactor != null)
        {
            _grabInteract.interactionManager.SelectExit(interactor, _grabInteract);
        }

        Debug.Log("Force Release");
    }


}