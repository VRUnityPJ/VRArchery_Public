using System;
using System.Linq;
using _VRArchery.Scripts.Utility;
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
    [SerializeField] public GameObject ArrowGrip;
    [SerializeField] private GameObject _arrowFeatherPoint;
    [SerializeField] YaFlyingManager yaFlyingManager;
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private XRGrabInteractable _grabInteract;
    private InputAction nockAction;
    private GameObject _bowString;
    private IBow _bow;

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
        CustomDebug.Log("nockAction enabled");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isNocking)
        {
            Vector3 direction = ArrowGrip.transform.position - this.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rb.rotation = targetRotation;
            transform.localRotation = Quaternion.identity;
            transform.LookAt(ArrowGrip.transform);
            if (_bowString != null)
            {
                _bowString.transform.position = _arrowFeatherPoint.transform.position;
                CustomDebug.Log("つかみ中！");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            //GameManager.instance.SetTargetModleMarker(collision.GetContact(0).point - collision.transform.position);
        }
        else
        {
            GameManager.instance.SetTargetModleMarker(Vector3.one * -1);
        }
        if (collision.gameObject.TryGetComponent(out IBow bow))
        {
            _bowString = bow.GetWirePointObject();
            _bow = bow;
            CustomDebug.Log("つかみ中！");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Nock"))
        {
            CustomDebug.Log(" Can Nock");
            _canNock = true;
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            //GameManager.instance.SetTargetModleMarker(collision.GetContact(0).point - collision.transform.position);
            _isFlying = false;
            _rb.isKinematic = true;
            _rb.useGravity = false;
            CustomDebug.Log($"刺さった:{_rb.isKinematic}");
            //_boxCollider.isTrigger = true;
        }
        if (other.gameObject.TryGetComponent(out IBow bow))
        {
            _bowString = bow.GetWirePointObject();
            _bow = bow;
            CustomDebug.Log("つかみ中！");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Nock"))
        {
            CustomDebug.Log("Cannot Nock");
            _canNock = false;
            //_grabInteract.trackRotation = true;
        }
    }

    private void OnTriggerRightPressed(InputAction.CallbackContext ctx)
    {
        CustomDebug.Log("OnTriggerRightPressed");
        if (_canNock)
        {
            CustomDebug.Log("An Arrow Is Nocking");
            _isNocking = true;
            _grabInteract.trackRotation = false;
            _rb.useGravity = false;
            _isFlying = false;
        }
    }

    private void OnTriggerRightReleased(InputAction.CallbackContext ctx)
    {
        CustomDebug.Log("OnTriggerRightReleased");
        if (_isNocking)
        {
            CustomDebug.Log("An Arrow is Shooted");
            _grabInteract.trackRotation = true;
            _isNocking = false;
            ForceRelease();
            yaFlyingManager.IsFlying = true;
            _rb.useGravity = true;
            if (ArrowGrip.transform.position != null)
            {
                Debug.Log($"Vector: {ArrowGrip.transform.position - this.transform.position}");
                _rb.AddForce((ArrowGrip.transform.position - this.transform.position) * _speed, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("No Arrow Grip");
            }
            if (_bow != null)
            {
                _bow.ResetWirePointObject();
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