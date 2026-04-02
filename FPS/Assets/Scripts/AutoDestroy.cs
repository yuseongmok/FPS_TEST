using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 1.5f; // 1초 뒤 삭제
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}