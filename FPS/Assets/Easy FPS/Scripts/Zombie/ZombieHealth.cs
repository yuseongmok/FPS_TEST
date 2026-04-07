using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f; // 좀비 체력
    public GameObject bloodEffect; // 피격 시 파티클

    // 데미지를 받는 함수
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("좀비 체력: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 죽을 때 효과음이나 애니메이션 처리
        Debug.Log("좀비 사망!");
        Destroy(gameObject); // 일단은 오브젝트 삭제
    }
}