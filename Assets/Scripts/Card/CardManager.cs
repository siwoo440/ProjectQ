using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats; // 플레이어 스탯을 저장한다.
    public GameObject bulletPrefab; // 생성할 탄환 프리팹을 저장한다.
    public Transform firePoint; // 탄환이 생성될 위치를 저장한다.
    public Camera mainCamera; // 마우스 위치 계산에 사용할 카메라를 저장한다.

    [Header("Pixel Shot Settings")]
    public float manaCost = 1f; // 픽셀 샷 마나 비용을 저장한다.
    public float cooldown = 1f; // 픽셀 샷 쿨타임을 저장한다.
    public int bulletDamage = 10; // 탄환 1발의 피해량을 저장한다.
    public float bulletSpeed = 10f; // 탄환 속도를 저장한다.
    public float bulletLifeTime = 3f; // 탄환 생존 시간을 저장한다.
    public int bulletCount = 3; // 발사할 탄환 수를 저장한다.
    public float spreadAngle = 10f; // 탄환 사이의 각도 차이를 저장한다.

    private float nextUseTime = 0f; // 다음 카드 사용 가능 시간을 저장한다.

    private void Start() // 시작 시 필요한 참조를 자동으로 보완한다.
    {
        if (mainCamera == null) // 카메라가 연결되지 않았는지 확인한다.
        {
            mainCamera = Camera.main; // MainCamera 태그가 붙은 카메라를 가져온다.
        }

        if (playerStats == null) // 플레이어 스탯이 연결되지 않았는지 확인한다.
        {
            playerStats = FindFirstObjectByType<PlayerStats>(); // 씬에서 PlayerStats를 찾아서 연결한다.
        }
    }

    private void Update() // 매 프레임 카드 사용 입력을 확인한다.
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭을 눌렀는지 확인한다.
        {
            TryUsePixelShot(); // 픽셀 샷 사용을 시도한다.
        }
    }

    private void TryUsePixelShot() // 픽셀 샷을 사용할 수 있는지 확인한다.
    {
        if (Time.time < nextUseTime) // 아직 쿨타임 중인지 확인한다.
        {
            Debug.Log("카드 쿨타임 중"); // 쿨타임 로그를 출력한다.
            return; // 카드 사용을 중단한다.
        }

        if (playerStats == null) // 플레이어 스탯이 없는지 확인한다.
        {
            Debug.LogError("PlayerStats가 연결되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 카드 사용을 중단한다.
        }

        if (bulletPrefab == null) // 탄환 프리팹이 없는지 확인한다.
        {
            Debug.LogError("Bullet Prefab이 연결되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 카드 사용을 중단한다.
        }

        if (firePoint == null) // 발사 위치가 없는지 확인한다.
        {
            Debug.LogError("FirePoint가 연결되지 않았습니다."); // 오류 로그를 출력한다.
            return; // 카드 사용을 중단한다.
        }

        bool canUseMana = playerStats.UseMana(manaCost); // 마나 사용을 시도한다.

        if (canUseMana == false) // 마나 사용에 실패했는지 확인한다.
        {
            return; // 카드 사용을 중단한다.
        }

        FirePixelShot(); // 픽셀 샷 탄환을 발사한다.
        nextUseTime = Time.time + cooldown; // 다음 사용 가능 시간을 설정한다.
    }

    private void FirePixelShot() // 픽셀 샷 3발을 발사한다.
    {
        Vector2 baseDirection = GetMouseDirection(); // 마우스 방향을 기준 방향으로 가져온다.

        for (int i = 0; i < bulletCount; i++) // 탄환 수만큼 반복한다.
        {
            float centerIndex = (bulletCount - 1) / 2f; // 가운데 탄환의 기준 인덱스를 계산한다.
            float angleOffset = (i - centerIndex) * spreadAngle; // 각 탄환의 각도 차이를 계산한다.
            Vector2 shotDirection = RotateVector(baseDirection, angleOffset); // 기준 방향을 각도만큼 회전한다.

            CreateBullet(shotDirection); // 회전된 방향으로 탄환을 생성한다.
        }
    }

    private Vector2 GetMouseDirection() // 마우스 방향을 계산한다.
    {
        Vector3 mousePosition = Input.mousePosition; // 화면상의 마우스 위치를 가져온다.
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // 마우스 위치를 월드 좌표로 변환한다.
        mouseWorldPosition.z = 0f; // 2D 게임이므로 z값을 0으로 맞춘다.

        Vector2 direction = mouseWorldPosition - firePoint.position; // 발사 위치에서 마우스까지의 방향을 계산한다.

        if (direction.sqrMagnitude <= 0.001f) // 방향 값이 너무 작은지 확인한다.
        {
            direction = Vector2.right; // 기본 방향을 오른쪽으로 설정한다.
        }

        return direction.normalized; // 방향을 정규화해서 반환한다.
    }

    private Vector2 RotateVector(Vector2 vector, float angle) // 벡터를 지정 각도만큼 회전한다.
    {
        float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환한다.
        float cos = Mathf.Cos(radian); // 코사인 값을 계산한다.
        float sin = Mathf.Sin(radian); // 사인 값을 계산한다.

        float x = vector.x * cos - vector.y * sin; // 회전된 x값을 계산한다.
        float y = vector.x * sin + vector.y * cos; // 회전된 y값을 계산한다.

        return new Vector2(x, y).normalized; // 회전된 방향을 정규화해서 반환한다.
    }

    private void CreateBullet(Vector2 direction) // 탄환을 생성한다.
    {
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // 탄환 프리팹을 생성한다.
        Bullet bullet = bulletObject.GetComponent<Bullet>(); // 생성된 탄환의 Bullet 컴포넌트를 가져온다.

        if (bullet == null) // Bullet 컴포넌트가 없는지 확인한다.
        {
            Debug.LogError("생성된 탄환에 Bullet.cs가 없습니다."); // 오류 로그를 출력한다.
            return; // 탄환 설정을 중단한다.
        }

        bullet.Initialize(direction, bulletSpeed, bulletDamage, bulletLifeTime); // 탄환의 방향, 속도, 피해량, 생존 시간을 설정한다.
    }
}