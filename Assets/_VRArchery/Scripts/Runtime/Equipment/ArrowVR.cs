using System;
using System.Linq;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class ArrowVR : MonoBehaviour
    {
        [SerializeField] private GameObject _arrow;
        [SerializeField] private float _speed = 40;
        [SerializeField] private GameObject _arrowFeatherPoint;
        [SerializeField] private ArrowFaceMovement _arrowFaceMovement;
        [SerializeField] private InputActionAsset _actionAsset;
        [SerializeField] private XRGrabInteractable _grabInteract;
        private bool _isFlying;
        private bool _isNocking = false;
        private bool _canNock = false;
        private Vector3 _prePosition;
        private Rigidbody _rb;
        private BoxCollider _boxCollider;
        private InputAction _nockAction;
        private GameObject _bowString;
        private IBow _bow;
        /// <summary>
        /// 矢をつがえる位置の当たり判定
        /// </summary>
        public GameObject ArrowGrip;
        public ArrowCounter ArrowCounter;

        public ArrowGrabber ArrowGrabber;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            var actionMap = _actionAsset.FindActionMap("Archery");
            if (actionMap != null)
            {
                _nockAction = actionMap.FindAction("nockArrow", true);
                Debug.Log("nock arrow attached");
                if (_nockAction == null)
                {
                    Debug.LogError("Cannot find nock arrow");
                }
            }
            else
            {
                Debug.LogError("No action asset found");
            }
        }
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
            _grabInteract.trackRotation = true;
        }

        private void OnEnable()
        {
            if (_nockAction == null)
                return;
            _nockAction.Enable();

            _nockAction.performed += OnTriggerRightPressed;
            _nockAction.canceled += OnTriggerRightReleased;
            CustomDebug.Log("nockAction enabled");
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isNocking)
            {
                Vector3 direction = ArrowGrip.transform.position - this.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _rb.rotation = targetRotation;
                transform.localRotation = Quaternion.identity;
                transform.LookAt(ArrowGrip.transform);
                if (_bowString)
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
                //GameManager.instance.SetTargetModleMarker(Vector3.one * -1);
            }
            if (collision.gameObject.TryGetComponent(out IBow bow))
            {
                _bowString = bow.GetWirePointObject();
                _bow = bow;
                CustomDebug.Log("つかみ中！");
            }


        }

        private async void OnTriggerEnter(Collider other)
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
                await DelayDestroyAsync();
                //_boxCollider.isTrigger = true;
            }
            else if (other.gameObject.CompareTag("Stage"))
            {
                DelayDestroyAsync().Forget();
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

        async private void OnTriggerRightReleased(InputAction.CallbackContext ctx)
        {
            CustomDebug.Log("OnTriggerRightReleased");
            if (_isNocking)
            {
                CustomDebug.Log("An Arrow is Shooted");
                _grabInteract.trackRotation = true;
                _isNocking = false;
                ForceRelease();
                _arrowFaceMovement.IsFlying = true;
                _rb.useGravity = true;
                ArrowCounter.AddArrowCount();
                Debug.Log($"Vector: {ArrowGrip.transform.position - this.transform.position}");
                _rb.AddForce((ArrowGrip.transform.position - this.transform.position) * _speed, ForceMode.Impulse);
                if (_bow != null)
                {
                    _bow.ResetWirePointObject();
                }

                await ReloadArrowAsync();
            }
        }

        private void ForceRelease()
        {
            var interactor = _grabInteract.interactorsSelecting.FirstOrDefault();
            if (interactor != null)
            {
                _grabInteract.interactionManager.SelectExit(interactor, _grabInteract);
            }
        }

        private async UniTask ReloadArrowAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            ArrowGrabber.GrabArrow(ArrowGrabber.ArrowGrabHand);
        }

        private async UniTask DelayDestroyAsync()
        {
            _rb.isKinematic = true;
            _boxCollider.enabled = false;

            // CancellationToken を取得
            var token = this.GetCancellationTokenOnDestroy();

            // 2秒待つ。ただし、待っている間にオブジェクトが破壊されたら、
            // 例外を発生させて処理を中断する
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);

            // このオブジェクトがまだ存在していれば破壊する
            // (awaitで例外が投げられた場合、ここには到達しない)
            Destroy(gameObject);
        }
    }
}