using System;
using UnityEditor;
using UnityEngine;

// <summary>
// Класс для описания поведения игровой модели персонажа
// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float inertiaDecayRate = 0f; // скорость затухания инерции

    [Header("Inertia Settings")]
    public float inertiaDecayRateGround = 3f; // скорость затухания обычной инерции на земле
    public float dashDecayRate = 10f; // скорость затухания после дэша

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); // размер области проверки земли
    public LayerMask whatIsGround;

    [Header("Physics Settings")]
    public float groundFriction = 5f; // трение на земле
    public PhysicsMaterial2D zeroFrictionMaterial;
    public PhysicsMaterial2D highFrictionMaterial;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    public bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private bool isBoostedFromDash;
    private bool isStopped;
    private bool isSlowed;

    private float dashTime;
    private float nextDashTime;
    private float moveX;
    private float currentMoveSpeed;
    private float originalMoveSpeed;

    private int facingDirection = 1;
    private Vector2 groundNormal = Vector2.up; // нормаль текущей поверхности

    private Coroutine slowCoroutine;

    private void Start()
    {
        Application.targetFrameRate = 90;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        originalMoveSpeed = moveSpeed;
        currentMoveSpeed = moveSpeed;

        // Создание материалов при необходимости
        if (zeroFrictionMaterial == null)
            zeroFrictionMaterial = new PhysicsMaterial2D("ZeroFriction") { friction = 0f, bounciness = 0f };

        if (highFrictionMaterial == null)
            highFrictionMaterial = new PhysicsMaterial2D("HighFriction") { friction = groundFriction, bounciness = 0f };

        // Устанавливаем материал без трения на старте
        col.sharedMaterial = zeroFrictionMaterial;
    }

    private void Update()
    {
        if (isStopped)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        moveX = Input.GetAxisRaw("Horizontal");

        if (moveX != 0)
        {
            facingDirection = (int)Mathf.Sign(moveX);
            spriteRenderer.flipX = moveX < 0;
        }

        if (!isDashing)
        {
            rb.velocity = new Vector2(moveX * currentMoveSpeed, rb.velocity.y);

            // Затухание скорости
            if (isGrounded && currentMoveSpeed > originalMoveSpeed)
            {
                float decayRate = isBoostedFromDash ? dashDecayRate : inertiaDecayRateGround;
                currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, originalMoveSpeed, decayRate * Time.deltaTime);

                if (Mathf.Approximately(currentMoveSpeed, originalMoveSpeed))
                    isBoostedFromDash = false;
            }
        }

        GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space))
            HandleJump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= nextDashTime && !isSlowed)
            StartDash();

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
                EndDash();
        }

        animator.SetFloat("Speed", Mathf.Abs(moveX));
    }

    //<Summary>
    //Обработка прыжка и двойного прыжка
    //</Summary>
    private void HandleJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = true;
            animator.SetBool("IsJumping", true);
        }
        else if (canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = false;
            animator.SetBool("IsJumping", true);
        }
    }

    //<Summary>
    //Остановка игрока на время
    //</Summary>
    public void StopMovement(float duration)
    {
        if (isStopped) return;

        isStopped = true;
        currentMoveSpeed = originalMoveSpeed;
        isBoostedFromDash = false;
        Invoke(nameof(ResumeMovement), duration);
    }

    //<Summary>
    //Замедление игрока на время
    //</Summary>
    public void SlowMovement(float duration, float slowFactor)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        currentMoveSpeed = originalMoveSpeed * slowFactor;
        isBoostedFromDash = false;
        isSlowed = true;
        slowCoroutine = StartCoroutine(SlowRoutine(duration));
    }

    //<Summary>
    //Рутина замедления игрока
    //</Summary>
    private System.Collections.IEnumerator SlowRoutine(float duration)
    {
        moveSpeed = currentMoveSpeed;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
        currentMoveSpeed = originalMoveSpeed;
        isSlowed = false;
    }


    //<Summary>
    //Разрешение движения после остановки
    //</Summary>
    private void ResumeMovement()
    {
        isStopped = false;
    }

    //<Summary>
    //Проверка на землю
    //</Summary>
    private void GroundCheck()
    {
        const float maxSlopeAngle = 75f;
        RaycastHit2D hit = Physics2D.BoxCast(
            groundCheck.position,
            groundCheckSize,
            0f,
            Vector2.down,
            0f,
            whatIsGround
        );

        if (hit.collider != null)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            isGrounded = slopeAngle <= maxSlopeAngle;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector2.up;
        }

        UpdateFriction();
    }

    //<Summary>
    //Обновление трения в зависимости от состояния
    //</Summary>
    private void UpdateFriction()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
            animator.SetBool("IsJumping", false);
            col.sharedMaterial = Mathf.Abs(moveX) > 0.01f ? zeroFrictionMaterial : highFrictionMaterial;
        }
        else
        {
            col.sharedMaterial = zeroFrictionMaterial;
        }
    }

    //<Summary>
    //Запуск дэша
    //</Summary>
    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        nextDashTime = Time.time + dashCooldown;

        Vector2 dashDirection = new Vector2(groundNormal.y, -groundNormal.x).normalized * facingDirection;
        rb.velocity = dashDirection * dashForce;

        currentMoveSpeed = Mathf.Max(currentMoveSpeed, dashForce);
        isBoostedFromDash = true;

        animator.SetBool("IsDashing", true);
        animator.SetInteger("LastMoveX", facingDirection);
    }

    //<Summary>
    //Окончание дэша
    //</Summary>
    private void EndDash()
    {
        isDashing = false;
        animator.SetBool("IsDashing", false);
    }

    //<Summary>
    //Отрисовка области проверки земли
    //</Summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        // Отрисовка области проверки земли
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

        // Если в редакторе, дополнительно рисуем направления
        if (Application.isPlaying)
        {
            // Позиция игрока
            Vector3 playerPos = transform.position;

            // Вектор скорости
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerPos, playerPos + (Vector3)(rb.velocity * 0.5f));
            Handles.Label(playerPos + (Vector3)(rb.velocity * 0.5f), $"Velocity: {rb.velocity}");

            // Вектор нормали поверхности
            Gizmos.color = Color.red; 
            Gizmos.DrawLine(playerPos, playerPos + (Vector3)(groundNormal * 1f));
            Handles.Label(playerPos + (Vector3)(groundNormal * 1f), $"Ground Normal: {groundNormal}");

            // Дирекция движения
            Vector2 moveDir = new Vector2(facingDirection, 0);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(playerPos, playerPos + (Vector3)(moveDir * 1f));
            Handles.Label(playerPos + (Vector3)(moveDir * 1f), $"Facing: {(facingDirection == 1 ? "Right" : "Left")}");

            // Состояния
            Gizmos.color = Color.white;
            Handles.Label(playerPos + Vector3.up * 2f, $"Grounded: {isGrounded}\nDashing: {isDashing}\nMoveSpeed: {currentMoveSpeed:F2}");
        }
    }

}