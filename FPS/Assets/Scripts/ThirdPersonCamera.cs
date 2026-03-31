using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;          // 추적할 대상 (Player)
    public Vector3 offset = new Vector3(0, 2f, -5f); // 캐릭터와의 기본 거리

    [Header("Rotation Settings")]
    public float mouseSensitivity = 3.0f; // 마우스 감도
    public float verticalMinAngle = -20f; // 위로 보는 제한
    public float verticalMaxAngle = 60f;  // 아래로 보는 제한

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // 마우스 커서를 고정하고 숨깁니다.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 카메라 이동은 캐릭터 이동이 끝난 후인 LateUpdate에서 처리하는 게 부드럽습니다.
    void LateUpdate()
    {
        if (target == null) return;

        // 1. 마우스 입력 받기
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 2. 위아래 회전각도 제한 (Clamp)
        rotationX = Mathf.Clamp(rotationX, verticalMinAngle, verticalMaxAngle);

        // 3. 회전값 적용
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        // 4. 카메라 위치 계산 (타겟 위치 + 회전된 오프셋)
        // 캐릭터의 머리 위쪽을 기준으로 하기 위해 offset을 더함
        transform.position = target.position + rotation * offset;

        // 5. 항상 타겟을 바라보게 함
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}