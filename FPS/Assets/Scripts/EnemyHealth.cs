using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("적 체력: " + currentHealth);

        // 피격 애니메이션이 있다면 여기서 실행 (예: anim.SetTrigger("Hurt"))

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("적이 죽었습니다!");
        Destroy(gameObject); // 일단은 파괴 처리
    }
}