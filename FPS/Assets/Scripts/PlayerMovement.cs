using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private Transform cameraTransform;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Movement Settings")]
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.2f;
    public float gravityValue = -19.62f;
    public float rotationSpeed = 10f;

    [Header("Combat Settings")]
    public float attackDelay = 0.5f;   // 공격 애니메이션 길이에 맞춰 조절
    public int maxCombo = 3;           // 최대 콤보 수
    public float attackRange = 2.0f;   // 캐릭터로부터 판정 원의 거리
    public float attackRadius = 1.5f;  // 판정 원의 크기
    public LayerMask enemyLayer;       // 적을 감지할 레이어 (Enemy)
    public int damage = 10;            // 공격력

    [Header("Effect Settings")]
    public GameObject[] attackEffects; // 콤보별 이펙트 프리팹들을 넣을 배열

    private bool isAttacking = false;
    private int comboStep = 0;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. 바닥 체크
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }

        // 2. 공격 입력 감지 (우클릭: 1)
        if (Input.GetMouseButtonDown(1) && groundedPlayer && !isAttacking)
        {
            Attack();
        }

        // 3. 이동 및 회전
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f && !isAttacking)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 4. 점프
        if (Input.GetButtonDown("Jump") && groundedPlayer && !isAttacking)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // 5. 중력 적용
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // 6. 애니메이션 업데이트
        if (anim != null)
        {
            float speedForAnim = isAttacking ? 0 : inputDirection.magnitude;
            anim.SetFloat("MoveSpeed", speedForAnim);
        }
    }

    public void SpawnAttackEffect(int effectIndex)
    {
        if (attackEffects == null || attackEffects.Length <= effectIndex) return;
        if (attackEffects[effectIndex] == null) return;

        // 플레이어 위치 + 앞쪽으로 살짝 띄워서 이펙트 생성
        Quaternion spawnRotation = transform.rotation;
        Vector3 spawnPosition = transform.position + transform.forward * 0.5f + Vector3.up * 0.5f;

        Instantiate(attackEffects[effectIndex], spawnPosition, spawnRotation);
    }

    private void Attack()
    {
        if (anim != null)
        {
            // 콤보 설정 전달
            anim.SetInteger("ComboIndex", comboStep);
            anim.SetTrigger("Attack");

            isAttacking = true;

            // --- 공격 판정 실행 ---
            CheckForHit();

            // 콤보 인덱스 증가
            comboStep++;
            if (comboStep >= maxCombo) comboStep = 0;

            // 선후딜 리셋
            Invoke("ResetAttackStatus", attackDelay);

            // 콤보 초기화 예약 (1.5초 동안 입력 없으면 0번으로)
            CancelInvoke("ResetCombo");
            Invoke("ResetCombo", 2.5f);
        }
    }

    // 실제로 적을 감지하고 데미지를 주는 로직
    private void CheckForHit()
    {
        // 플레이어 앞쪽(forward)으로 attackRange만큼 떨어진 곳에 반지름 attackRadius인 가상의 구체를 그려 충돌 감지
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " 히트!");

            // 적의 체력 스크립트를 가져와 데미지 전달
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void ResetAttackStatus()
    {
        isAttacking = false;
    }

    private void ResetCombo()
    {
        comboStep = 0;
        if (!isAttacking && anim != null) anim.SetInteger("ComboIndex", 0);
    }

    // 유니티 씬(Scene) 뷰에서 공격 범위를 빨간 원으로 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // 구체의 위치를 시각적으로 표시
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRadius);
    }
}