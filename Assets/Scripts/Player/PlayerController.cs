using UnityEngine; // 유니티 기능

public class PlayerController : MonoBehaviour // 플레이어 이동 관리
{
    [Header("Move")] // 이동 영역
    [SerializeField] private float moveSpeed = 5f; // 이동 속도

    private Rigidbody2D rb; // 물리 컴포넌트
    private PlayerDodge playerDodge; // 회피 컴포넌트
    private Vector2 moveInput; // 이동 입력

    private void Awake() // 초기 연결
    {
        rb = GetComponent<Rigidbody2D>(); // 물리 연결
        playerDodge = GetComponent<PlayerDodge>(); // 회피 연결
        Debug.Log("PlayerController 연결 완료"); // 연결 확인
    }

    private void Update() // 입력 갱신
    {
        ReadMoveInput(); // 이동 입력 읽기
        //if (moveInput != Vector2.zero)  { Debug.Log("이동 입력: " + moveInput); }// 입력 확인 -> 출력
    }

    private void FixedUpdate() // 물리 갱신
    {
        MovePlayer(); // 플레이어 이동
    }

    private void ReadMoveInput() // 이동 입력 처리 함수
    {
        moveInput = Vector2.zero; // 입력값 초기화

        if (Input.GetKey(KeyCode.W)) moveInput.y += 1f; // W 입력 -> 위 이동값 추가
        if (Input.GetKey(KeyCode.S)) moveInput.y -= 1f; // S 입력 -> 아래 이동값 추가
        if (Input.GetKey(KeyCode.A)) moveInput.x -= 1f; // A 입력 -> 왼쪽 이동값 추가
        if (Input.GetKey(KeyCode.D)) moveInput.x += 1f; // D 입력 -> 오른쪽 이동값 추가
        moveInput = moveInput.normalized; // 대각선 이동값 -> 속도 보정
    }

    private void MovePlayer() // 이동 적용
    {
        if (playerDodge != null && playerDodge.IsDodging) { return; } // 회피 상태 확인 -> 회피 중 이동 중단
        rb.linearVelocity = moveInput * moveSpeed; // 이동 속도 적용
    }
}