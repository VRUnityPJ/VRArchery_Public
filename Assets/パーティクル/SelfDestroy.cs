using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float lifeTime = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
