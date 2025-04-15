using System;
using UnityEngine;

/// <summary>
/// Класс для описания поведения игровой модели персонажа
/// </summary>
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
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;

    private float moveX;
    private int facingDirection = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        // Обновление направления взгляда
        if (moveX != 0)
            facingDirection = (int)Mathf.Sign(moveX);

        if (!isDashing)
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
            }
            //второй прыжок
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
        //деш на шифт
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
    // <summary>
    // проверка на столкновение с землей
    // </summary>
    private void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, whatIsGround);
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
    //Старт дэша
    //</Summary>
    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        rb.velocity = new Vector2(facingDirection * dashForce, 0f);
    }
    //<Summary>
    //Конец дэша
    //</Summary>
    private void EndDash()
    {
        isDashing = false;
    }

    /// <summary>
    /// Debug отрисовка гизмоса
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
