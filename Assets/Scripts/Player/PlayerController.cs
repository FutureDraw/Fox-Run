using System;
using UnityEngine;

// <summary>
// Класс для описания поведения игровой модели персонажа
// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashForce = 10f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private float dashTime;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); // Настраиваемый размер прямоугольника
    public LayerMask whatIsGround;

    private float moveX;
    private int facingDirection = 1;

    // Переменные для контроля скорости
    private float originalMoveSpeed;
    private bool isStopped = false;
    private Coroutine slowCoroutine;

    private void Start()
    {
        Application.targetFrameRate = 90;
        rb = GetComponent<Rigidbody2D>();
        originalMoveSpeed = moveSpeed; // Сохраняем оригинальную скорость
    }

    private void Update()
    {

        if (isStopped)
        {
            rb.velocity = Vector2.zero;
            return; // при остановке игрока не обрабатывается ввод
        }
        moveX = Input.GetAxisRaw("Horizontal");

        if (moveX != 0)
            facingDirection = (int)Mathf.Sign(moveX);

        if (!isDashing)
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        GroundCheck();

        //реализация прыжка на пробел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        //реализация дэша на шифт
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
                EndDash();
        }
    }

    //<Summary>
    //Остановка игрока
    //</Summary>
    public void StopMovement(float duration)
    {
        if (isStopped) return;
        isStopped = true;
        Invoke(nameof(ResumeMovement), duration);
    }

    //<Summary>
    //Замедление игрока
    //</Summary>
    public void SlowMovement(float duration, float slowFactor)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowRoutine(duration, slowFactor));
    }

    //<Summary>
    //Рутина замедления игрока
    //</Summary>
    private System.Collections.IEnumerator SlowRoutine(float duration, float slowFactor)
    {
        moveSpeed = originalMoveSpeed * slowFactor;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
    }

    //<Summary>
    //Разблокировка движения
    //</Summary>
    private void ResumeMovement()
    {
        isStopped = false;
        isGrounded = true;
    }

    //<Summary>
    //Проверка приземления
    //</Summary>
    private void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            groundCheck.position,
            groundCheckSize,
            0f,
            whatIsGround);

        isGrounded = false;

        foreach (Collider2D col in colliders)
        {
            Vector2 contactPoint = col.ClosestPoint(transform.position);
            if (contactPoint.y < transform.position.y - 0.1f)
            {
                isGrounded = true;
                break;
            }
        }
        if (isGrounded)
            canDoubleJump = true;
    }

    //<Summary>
    //Начало дэша
    //</Summary>
    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        rb.velocity = new Vector2(facingDirection * dashForce, 0f);
    }

    //<Summary>
    //Завершение дэша
    //</Summary>
    private void EndDash()
    {
        isDashing = false;
    }

    //<Summary>
    //Дебаг отрисовка
    //</Summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}