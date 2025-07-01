using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject _TargetModleCenter;
    public Vector3 pos;
    [SerializeField] private Text _scoreText;
    private int _score;
    [SerializeField] float _targetRadius;
    public bool _start;
    [SerializeField] public bool _isPlaying;
    private float _timer;
    [SerializeField] private GameObject _startCount;
    [SerializeField] private Text _startCountText;
    [SerializeField] private int _maxScore = 100;
    [SerializeField] private int _waitingTime = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_start)
        {
            if(_timer < _waitingTime)
            {
                _timer += Time.deltaTime;
                _startCountText.text = Mathf.FloorToInt(_waitingTime - _timer).ToString();
            }
            else
            {
                _start = false;
                _isPlaying = true;
                _startCountText.text = null;
                _startCount.gameObject.SetActive(false);
            }     
        }
    }

    public void SetTargetModleMarker(Vector3 hitPosition)
    {
        if(hitPosition == Vector3.one * -1)
        {
            _score = 0;
            _TargetModleCenter.SetActive(false);
        }
        else
        {
            _TargetModleCenter.SetActive(true);
            var hitPointDistance = hitPosition.magnitude;
            _TargetModleCenter.transform.localPosition = new Vector3(-hitPosition.x, hitPosition.y, 0);
            pos = _TargetModleCenter.transform.localPosition;

            if (hitPointDistance / _targetRadius < 0.15f)
            {
                _score = _maxScore;
            }
            else
            {
                if (hitPointDistance > _targetRadius)
                {
                    _score = 0;
                }
                else
                {
                    _score = Mathf.RoundToInt(_maxScore - _maxScore * hitPointDistance / _targetRadius);
                }
            }
        }

        _scoreText.text = _score.ToString();
    }

    public void GameStart()
    {
        _start = true;
    }
}