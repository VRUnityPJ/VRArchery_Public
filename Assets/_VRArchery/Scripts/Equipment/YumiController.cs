//using Unity.VisualScripting;
using UnityEngine;

public class YumiController : MonoBehaviour
{
    [SerializeField] private GameObject _Ya_prefab;
    private GameObject _Ya;
    [SerializeField] private float _yumiMoveSpeed;
    [SerializeField] private float _yaSpeed;
    private Rigidbody _rb;
    private Ya _ya;
    [SerializeField] Transform _Gen;
    private Vector3 _defGenPos;//弦の元の位置
    [SerializeField] private float _lifeTime = 10;
    [SerializeField] private float _interval = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _defGenPos = _Gen.localPosition;
        YaInstantiate();
    }

    // Update is called once per frame
    void Update()
    {         
        if(GameManager.instance._isPlaying)
        {
            if (Input.GetKey(KeyCode.W) && _Ya)
            {
                _Ya.transform.localPosition += new Vector3(0, 0, _yumiMoveSpeed) * Time.deltaTime;

            }
            if (Input.GetKey(KeyCode.S) && _Ya)
            {
                _Ya.transform.localPosition -= new Vector3(0, 0, _yumiMoveSpeed) * Time.deltaTime;
            }

            if (_Ya)
            {
                _Gen.position = _Ya.transform.position;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_Ya)
                {
                    _rb.isKinematic = false;
                    _rb.AddForce((gameObject.transform.position - _Ya.transform.position) * _yaSpeed, ForceMode.Impulse);
                    _Ya.transform.parent = null;
                    _ya._isFlying = true;
                    Destroy(_Ya, _lifeTime);
                    _Ya = null;
                    _rb = null;
                    _Gen.transform.localPosition = _defGenPos;
                    Invoke("YaInstantiate", _interval);
                }
            }
        }
    }

    private void YaInstantiate()
    {
        _Ya = Instantiate(_Ya_prefab,gameObject.transform);
        _Ya.transform.position = gameObject.transform.position;
        _Ya.transform.rotation = gameObject.transform.rotation;
        _rb = _Ya.GetComponent<Rigidbody>();
        _ya = _Ya.GetComponent<Ya>();
        _rb.isKinematic = true;
    }
    
}