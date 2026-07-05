using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    [Header("Dodge Settings")]
    public float dodgeSpeed = 12f; // 회피 중 이동 속도를 저장한다.
    public float dodgeTime = 0.2f; // 회피 이동 시간을 저장한다.
    public float invincibleTime = 0.3f; // 회피 무적 시간을 저장한다.
    public float dodgeCooldown = 1.2f; // 회피 쿨타임을 저장한다.

    public bool IsDodging { get; private set; } // 현재 회피 중인지 외부에서 확인하게 한다.
    public bool IsInvincible { get; private set; } // 현재 무적 상태인지 외부에서 확인하게 한다.

    private bool canDodge = true; // 회피 가능 여부를 저장한다.
    private Vector2 lastMoveDirection = Vector2.right; // 마지막 이동 방향을 저장한다.
    private Rigidbody2D rb; // 플레이어 Rigidbody2D를 저장한다.

    private void Awake() // 오브젝트가 시작될 때 Rigidbody2D를 가져온다.
    {
        rb = GetComponent<Rigidbody2D>(); // 현재 오브젝트의 Rigidbody2D를 가져온다.
    }

    private void Update() // 매 프레임 입력을 확인한다.
    {
        UpdateLastMoveDirection(); // 마지막 이동 방향을 갱신한다.

        if (Input.GetKeyDown(KeyCode.LeftShift)) // Left Shift를 눌렀는지 확인한다.
        {
            TryDodge(); // 회피를 시도한다.
        }
    }

    private void UpdateLastMoveDirection() // 마지막 이동 방향을 갱신한다.
    {
        Vector2 input = Vector2.zero; // 입력 방향을 초기화한다.

        if (Input.GetKey(KeyCode.W)) input.y += 1f; // W 입력 시 위 방향을 더한다.
        if (Input.GetKey(KeyCode.S)) input.y -= 1f; // S 입력 시 아래 방향을 더한다.
        if (Input.GetKey(KeyCode.A)) input.x -= 1f; // A 입력 시 왼쪽 방향을 더한다.
        if (Input.GetKey(KeyCode.D)) input.x += 1f; // D 입력 시 오른쪽 방향을 더한다.

        if (input.sqrMagnitude > 0.01f) // 입력 방향이 있는지 확인한다.
        {
            lastMoveDirection = input.normalized; // 마지막 이동 방향을 저장한다.
        }
    }

    private void TryDodge() // 회피 가능 여부를 확인하고 회피를 실행한다.
    {
        if (canDodge == false) return; // 회피 쿨타임 중이면 실행하지 않는다.

        StartCoroutine(DodgeRoutine()); // 회피 코루틴을 실행한다.
    }

    private IEnumerator DodgeRoutine() // 회피 이동과 무적 시간을 처리한다.
    {
        canDodge = false; // 회피를 잠시 사용할 수 없게 한다.
        IsDodging = true; // 회피 중 상태로 설정한다.
        IsInvincible = true; // 무적 상태로 설정한다.

        rb.linearVelocity = lastMoveDirection * dodgeSpeed; // 마지막 이동 방향으로 빠르게 이동한다.

        yield return new WaitForSeconds(dodgeTime); // 회피 이동 시간만큼 기다린다.

        IsDodging = false; // 회피 이동 상태를 해제한다.

        yield return new WaitForSeconds(invincibleTime - dodgeTime); // 남은 무적 시간만큼 기다린다.

        IsInvincible = false; // 무적 상태를 해제한다.

        yield return new WaitForSeconds(dodgeCooldown); // 회피 쿨타임만큼 기다린다.

        canDodge = true; // 회피를 다시 사용할 수 있게 한다.
    }
}