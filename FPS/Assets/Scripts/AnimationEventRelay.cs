using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerMovement playerMovement;

    void Start()
    {
        // 부모 오브젝트에 있는 PlayerMovement 스크립트를 찾습니다.
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // 애니메이션 이벤트가 호출하는 함수 이름과 똑같이 만듭니다.
    public void SpawnAttackEffect(int effectIndex)
    {
        if (playerMovement != null)
        {
            playerMovement.SpawnAttackEffect(effectIndex);
        }
    }
}