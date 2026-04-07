using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

    public float maxDistance = 1000000;
    RaycastHit hit;
    public GameObject decalHitWall;
    public float floatInfrontOfWall;
    public GameObject bloodEffect;
    public LayerMask ignoreLayer;

    // [추가] 총알의 데미지 설정
    public float damage = 25f;

    void Update()
    {
        // 레이캐스트 발사 (시작점, 방향, 결과저장, 거리, 레이어무시)
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {

            // 1. 벽(LevelPart)에 맞았을 때
            if (hit.transform.tag == "LevelPart")
            {
                if (decalHitWall)
                    Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
                Destroy(gameObject);
            }

            // 2. 적(좀비)에게 맞았을 때
            // 태그가 "Dummie" 혹은 아까 설정한 "Zombie"인지 확인합니다.
            if (hit.transform.tag == "Dummie" || hit.transform.tag == "Zombie")
            {

                // 피 효과 생성
                if (bloodEffect)
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

                // [핵심 추가] 상대방에게 ZombieHealth 스크립트가 있는지 확인하고 데미지를 줍니다.
                ZombieHealth zombie = hit.transform.GetComponent<ZombieHealth>();
                if (zombie != null)
                {
                    zombie.TakeDamage(damage);
                }

                Destroy(gameObject);
            }

            // 무엇이든 맞으면 총알 삭제
            Destroy(gameObject);
        }

        // 아무것도 안 맞더라도 0.1초 뒤엔 삭제 (최적화)
        Destroy(gameObject, 0.1f);
    }
}