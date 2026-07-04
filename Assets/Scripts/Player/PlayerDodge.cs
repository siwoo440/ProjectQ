using System.Collections; // 코루틴 기능
using UnityEngine; // 유니티 기능

public class PlayerDodge : MonoBehaviour // 플레이어 회피 관리
{
    [Header("Dodge")] // 회피 영역
    [SerializeField] private float dodgeSpeed = 12f; // 회피 속도
    [SerializeField] private float dodgeTime = 0.2f; // 회피 시간
    [SerializeField] private float invincibleTime = 0.3f; // 무적 시간
    [SerializeField] private float dodgeCooldown = 1.2f; // 회피 쿨타임

    private Rigidbody2D rb; // 물리 컴포넌트
    private Vector2 lastMoveDirection = Vector2.down; // 마지막 이동 방향
    private bool isDodging = false; // 회피 상태
    private bool isInvincible = false; // 무적 상태
    private bool canDodge = true; // 회피 가능 상태

    public bool IsDodging => isDodging; // 회피 상태 읽기
    public bool IsInvincible => isInvincible; // 무적 상태 읽기

    private void Awake() // 초기 연결
    {
        rb = GetComponent<Rigidbody2D>(); // 물리 연결
    }

    private void Update() // 입력 갱신
    {
        UpdateLastMoveDirection(); // 방향 저장

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDodge) // 회피 입력 확인
        {
            StartCoroutine(DodgeRoutine()); // 회피 실행
        }
    }

    private void UpdateLastMoveDirection() // 마지막 방향 저장
    {
        Vector2 inputDirection = Vector2.zero; // 입력 방향 초기값

        if (Input.GetKey(KeyCode.W)) // 위 입력
        {
            inputDirection.y += 1f; // 위 방향 추가
        }

        if (Input.GetKey(KeyCode.S)) // 아래 입력
        {
            inputDirection.y -= 1f; // 아래 방향 추가
        }

        if (Input.GetKey(KeyCode.A)) // 왼쪽 입력
        {
            inputDirection.x -= 1f; // 왼쪽 방향 추가
        }

        if (Input.GetKey(KeyCode.D)) // 오른쪽 입력
        {
            inputDirection.x += 1f; // 오른쪽 방향 추가
        }

        if (inputDirection != Vector2.zero) // 입력 존재 확인
        {
            lastMoveDirection = inputDirection.normalized; // 마지막 방향 갱신
        }
    }

    private IEnumerator DodgeRoutine() // 회피 코루틴
    {
        canDodge = false; // 회피 잠금

        isDodging = true; // 회피 시작

        isInvincible = true; // 무적 시작

        rb.linearVelocity = lastMoveDirection * dodgeSpeed; // 회피 속도 적용

        yield return new WaitForSeconds(dodgeTime); // 회피 시간 대기

        isDodging = false; // 회피 종료

        yield return new WaitForSeconds(invincibleTime - dodgeTime); // 남은 무적 대기

        isInvincible = false; // 무적 종료

        yield return new WaitForSeconds(dodgeCooldown); // 쿨타임 대기

        canDodge = true; // 회피 가능
    }
}