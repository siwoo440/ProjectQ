using UnityEngine; // Unity 기본 기능 사용

public class BossBullet : MonoBehaviour // 보스 탄환 클래스
{
    [Header("Movement Settings")] // 이동 설정 제목
    public float speed = 6f; // 탄환 이동 속도

    public float lifeTime = 6f; // 탄환 생존 시간

    [Header("Damage Settings")] // 피해 설정 제목
    public int damage = 1; // 플레이어에게 줄 피해량

    private Vector2 moveDirection = Vector2.down; // 탄환 이동 방향

    private bool hasHitPlayer = false; // 플레이어 피격 여부

    private void Start() // 시작 함수
    {
        Destroy(gameObject, lifeTime); // 생존 시간 후 삭제
    }

    private void Update() // 매 프레임 이동 함수
    {
        Vector3 moveAmount = moveDirection.normalized * speed * Time.deltaTime; // 이동량 계산

        transform.position += moveAmount; // 탄환 위치 이동
    }

    public void SetDirection(Vector2 direction) // 이동 방향 설정 함수
    {
        if (direction == Vector2.zero) // 방향값 없음 확인
        {
            moveDirection = Vector2.down; // 기본 아래 방향 설정
        }
        else // 방향값 있음
        {
            moveDirection = direction.normalized; // 정규화 방향 저장
        }

        transform.up = moveDirection; // 탄환 방향 회전
    }

    private void OnTriggerEnter2D(Collider2D other) // 충돌 처리 함수
    {
        if (hasHitPlayer) // 이미 플레이어를 맞췄는지 확인
        {
            return; // 중복 피해 방지
        }

        PlayerStats playerStats = other.GetComponentInParent<PlayerStats>(); // 플레이어 스탯 검색

        if (playerStats == null) // 플레이어가 아닌지 확인
        {
            return; // 충돌 처리 중단
        }

        hasHitPlayer = true; // 피격 처리 완료 표시

        playerStats.TakeDamage(damage); // 플레이어 피해 적용

        Destroy(gameObject); // 탄환 삭제
    }
}