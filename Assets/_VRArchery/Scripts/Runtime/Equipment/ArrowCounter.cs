using _VRArchery.Scripts.Utility;
using R3;
using UnityEngine;

public class ArrowCounter : MonoBehaviour
{
    public ReactiveProperty<int> ShootedArrowNum = new ReactiveProperty<int>(0);
    [SerializeField] private int _maxArrow = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShootedArrowNum.Subscribe(arrow =>
        {
            if (arrow == _maxArrow)
            {
                //リザルト表示メソッド
                CustomDebug.Log($"All Arrows Shooted");
            }
        });
    }

    public void AddArrowCount()
    {
        ShootedArrowNum.Value++;
        CustomDebug.Log($"the Number of Shooted Arrows : {ShootedArrowNum.Value}");
    }
    public void ResetArrowCount()
    {
        ShootedArrowNum.Value = 0;
    }
}
