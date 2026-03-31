using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private Transform cameraTransform; // 메인 카메라의 위치 정보

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Movement Settings")]
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.2f;
    public float gravityValue = -19.62f; // 조금 더 묵직한 중력
    public float rotationSpeed = 10f;

    private bool isAttacking = false;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        // 메인 카메라의 Transform을 참조합니다.
        cameraTransform = Camera.main.transform;

        // 게임 플레이 중 마우스 커서를 숨깁니다.
        Cursor.lockState = CursorLockMode.Locked;

        // 자식 오브젝트에 있는 Animator를 찾아옵니다.
        anim = GetComponentInChildren<Animator>();
    }

    // 공격 명령을 내리는 함수
    private void Attack()
    {
        if (anim != null)
        {
            // 애니메이터의 'Attack' 트리거를 작동시킵니다.
            anim.SetTrigger("Attack");
            // 공격 시작 시 이동을 막기 위해 true로 설정
            isAttacking = true;
            // 애니메이션 길이에 맞춰 isAttacking을 다시 false로 돌려주는 로직이 필요합니다.
            // 가장 간단한 방법은 Invoke를 사용하는 것입니다. (애니메이션 길이를 대략 계산해서 넣으세요)
            // 예: 공격 애니메이션이 0.5초라면 0.5f를 넣습니다.
            Invoke("ResetAttackStatus", 0.5f);
        }
    }

    // 공격 상태를 초기화하는 함수 (Invoke에 의해 호출됨)
    private void ResetAttackStatus()
    {
        isAttacking = false;
    }
    void Update()
    {

        // 마우스 왼쪽 클릭(0)을 누르고, 공중에 떠 있지 않으며, 이미 공격 중이 아닐 때
        if (Input.GetMouseButtonDown(0) && groundedPlayer && !isAttacking)
        {
            Attack();
        }

        // 1. 바닥 체크
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f; // 바닥에 완전히 붙도록 살짝 누름
        }

        // 2. 입력 받기 및 방향 계산
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f && !isAttacking)
        {
            // 카메라의 Y축 회전값만 추출하여 이동 방향 계산
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // 캐릭터 이동
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);

            // 캐릭터 회전 (부드럽게 이동 방향 바라보기)
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 3. 점프
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // 4. 중력 적용
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // 5. 애니메이션 업데이트
        if (anim != null)
        {
            // 이동 애니메이션 업데이트
            anim.SetFloat("MoveSpeed", inputDirection.magnitude);
        }
    }
}