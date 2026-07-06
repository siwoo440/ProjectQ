using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int contactDamage = 10; // 접촉 피해량을 저장한다.
    public float damageCooldown = 1f; // 접촉 피해 간격을 저장한다.

    private float lastDamageTime = -999f; // 마지막으로 피해를 준 시간을 저장한다.

    private void OnTriggerEnter2D(Collider2D other) // 트리거에 처음 닿았을 때 실행한다.
    {
        TryDamagePlayer(other.gameObject); // 플레이어 피해를 시도한다.
    }

    private void OnTriggerStay2D(Collider2D other) // 트리거에 계속 닿아 있을 때 실행한다.
    {
        TryDamagePlayer(other.gameObject); // 플레이어 피해를 시도한다.
    }

    private void OnCollisionEnter2D(Collision2D collision) // 충돌이 처음 발생했을 때 실행한다.
    {
        TryDamagePlayer(collision.gameObject); // 플레이어 피해를 시도한다.
    }

    private void OnCollisionStay2D(Collision2D collision) // 충돌이 계속 유지될 때 실행한다.
    {
        TryDamagePlayer(collision.gameObject); // 플레이어 피해를 시도한다.
    }

    private void TryDamagePlayer(GameObject targetObject) // 플레이어에게 피해를 주는지 확인한다.
    {
        if (targetObject.CompareTag("Player") == false) return; // Player 태그가 아니면 실행하지 않는다.

        if (Time.time < lastDamageTime + damageCooldown) return; // 피해 쿨타임 중이면 실행하지 않는다.

        PlayerDodge playerDodge = targetObject.GetComponent<PlayerDodge>(); // 플레이어 회피 컴포넌트를 가져온다.

        if (playerDodge != null && playerDodge.IsInvincible) // 플레이어가 무적 상태인지 확인한다.
        {
            return; // 무적 상태면 피해를 주지 않는다.
        }

        PlayerStats playerStats = targetObject.GetComponent<PlayerStats>(); // 플레이어 스탯 컴포넌트를 가져온다.

        if (playerStats == null) return; // 플레이어 스탯이 없으면 실행하지 않는다.

        playerStats.TakeDamage(contactDamage); // 플레이어에게 접촉 피해를 준다.
        lastDamageTime = Time.time; // 마지막 피해 시간을 갱신한다.

        Debug.Log("Enemy Contact Damage : " + contactDamage); // 접촉 피해 로그를 출력한다.
    }
}