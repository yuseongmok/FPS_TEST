using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 2.0f; // 스폰 간격
    public Vector3 spawnRange = new Vector3(20, 0, 20);
    public int maxZombieCount = 50; // 최대 좀비 수 제한

    void Start()
    {
        // 2초 뒤부터 spawnInterval마다 SpawnZombie 함수 실행
        InvokeRepeating("SpawnZombie", 2.0f, spawnInterval);
    }

    void SpawnZombie()
    {
        // 1. 현재 씬에 있는 "Zombie" 태그를 가진 오브젝트들을 모두 찾음
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        // 2. 현재 좀비 숫자가 최대치(50)보다 적을 때만 생성
        if (zombies.Length < maxZombieCount)
        {
            float randomX = Random.Range(-spawnRange.x, spawnRange.x);
            float randomZ = Random.Range(-spawnRange.z, spawnRange.z);
            Vector3 randomPos = new Vector3(randomX, 0, randomZ) + transform.position;

            Instantiate(zombiePrefab, randomPos, Quaternion.identity);

            // 디버그 창에서 현재 좀비 수 확인용 (선택 사항)
            Debug.Log("현재 좀비 수: " + (zombies.Length + 1));
        }
        else
        {
            Debug.Log("좀비가 너무 많습니다. 스폰을 건너뜁니다.");
        }
    }
}