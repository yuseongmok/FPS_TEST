using UnityEngine;
using UnityEngine.AI; // NavMesh 사용을 위해 필수

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // "Player" 태그를 가진 오브젝트를 찾아 추격 대상으로 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 실시간으로 플레이어의 위치를 목적지로 설정
            agent.SetDestination(playerTransform.position);
        }
    }
}