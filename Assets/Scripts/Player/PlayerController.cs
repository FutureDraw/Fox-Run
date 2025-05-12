using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс для описания поведения игровой модели персонажа
/// </summary>
public class PlayerController : MonoBehaviour
{
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference dash;
    public InputActionReference interaction;
    public InputActionReference grab;
    private Vector2 _moveInput;

    [Header("Movement Settings")]   
    public float moveSpeed = 3f;
    public float jumpForce = 10f;
    public float dashForce = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float inertiaDecayRate = 5f; // скорость затухания инерции
    public static int JumpCount { get; private set; } // счётчик прыжков

    [Header("Inertia Settings")]
    public float inertiaDecayRateGround = 3f; // скорость затухания обычной инерции на земле
    public float dashDecayRate = 10f; // скорость затухания после дэша

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.34f, 0.04f); // размер области проверки земли
    public LayerMask whatIsGround;

    [Header("Wall Grab Settings")]
    public LayerMask wallLayer; // слой стен
    public float wallCheckDistance = 0.22f; // дистанция для проверки стены
    private int wallGrabCount = 0;
    private const int maxWallGrabs = 3;

    [Header("Physics Settings")]
    public float groundFriction = 5f; // трение на земле
    public PhysicsMaterial2D zeroFrictionMaterial;
    public PhysicsMaterial2D highFrictionMaterial;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    public bool isGrounded;
    public bool isGravityFlipped = false;
    private bool canDoubleJump;
    private bool isDashing;
    private bool isBoostedFromDash;
    private bool isStopped;
    private bool isSlowed;
    private bool isGrabbingWall;
    private bool isNearWall;
    private int wallDirection; // направление стены (-1 влево, 1 вправо)

    private float dashTime;
    private float nextDashTime;
    private float moveX;
    private float currentMoveSpeed;
    private float originalMoveSpeed;

    private int facingDirection = 1;
    private Vector2 groundNormal = Vector2.up; // нормаль текущей поверхности
    private bool wasJumping = false;

    private Coroutine slowCoroutine;

    private void Start()
    {
        move.action.Enable();
        jump.action.Enable();
        dash.action.Enable();
        interaction.action.Enable();
        grab.action.Enable();

        Application.targetFrameRate = 90;
        JumpCount = 0;
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

        if (!isDashing && !isGrabbingWall)
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
        WallCheck();

        HandleWallGrabInput();

        if (jump.action.triggered)
            HandleJump();

        if (dash.action.triggered && !isDashing && Time.time >= nextDashTime && !isSlowed && !isGrabbingWall)
            StartDash();

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
                EndDash();
        }

        // Получаем значение оси для движения
        _moveInput = move.action.ReadValue<Vector2>();
        moveX = _moveInput.x;  

        // Проверка на движение по оси X
        float speed = Mathf.Abs(moveX) > 0.1f && !isGrabbingWall ? Mathf.Abs(moveX) * currentMoveSpeed : 0f;

        // Смена направления персонажа в зависимости от движения
        if (speed > 0)
        {
            facingDirection = (int)Mathf.Sign(moveX);
            spriteRenderer.flipX = moveX < 0;
        }

        // Обновление параметра анимации Speed
        animator.SetFloat("Speed", speed); // Устанавливаем параметр Speed в аниматоре

        if (isGrounded && rb.velocity.y <= 0.1f)
        {
            animator.SetBool("IsJumping", false);
        }

        animator.SetFloat("Speed", isGrabbingWall ? 0f : Mathf.Abs(moveX));
    }

    //<Summary>
    //Обработка прыжка и двойного прыжка
    //</Summary>
    private void HandleJump()
    {
        if (isGrabbingWall)
        {
            Debug.Log("[WallJump] Прыжок со стены!");
            JumpCount++;
            rb.velocity = new Vector2(-wallDirection * moveSpeed, jumpForce);
            isGrabbingWall = false;
            canDoubleJump = true; // Разрешаем двойной прыжок после прыжка от стены
            wasJumping = true;
            animator.SetBool("IsJumping", true);
        }
        else if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            JumpCount++;
            canDoubleJump = true;
            wasJumping = true;
            animator.SetBool("IsJumping", true);
        }
        else if (canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            JumpCount++;
            canDoubleJump = false;
            wasJumping = true;

            animator.SetTrigger("JumpTrigger");
        }
    }

    //<Summary>
    //Проверка на зажатие/отжатие стены
    //</Summary>
    private void HandleWallGrabInput()
    {
        if (grab.action.triggered)
        {
            if (!isGrabbingWall && wallGrabCount < maxWallGrabs && isNearWall)
            {
                isGrabbingWall = true;
                rb.velocity = Vector2.zero;
                animator.SetBool("IsJumping", false);
                Debug.Log($"[WallGrab] Захват стены #{wallGrabCount + 1}");
                wallGrabCount++;
            }
            else if (isGrabbingWall)
            {
                // Повторное нажатие — отпустить стену
                isGrabbingWall = false;
                Debug.Log("[WallGrab] Отпущена стена");
            }
        }


        if (isGrabbingWall)
        {
            float wallSlideSpeed = 0.9f;
            rb.velocity = new Vector2(0, wallSlideSpeed);
        }
    }

    //<Summary>
    //Проверка на наличие стены рядом
    //</Summary>
    private void WallCheck()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);

        if (hitLeft.collider != null)
        {
            isNearWall = true;
            wallDirection = -1;
        }
        else if (hitRight.collider != null)
        {
            isNearWall = true;
            wallDirection = 1;
        }
        else
        {
            isNearWall = false;
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
        animator.SetBool("IsIdle", false);
        Debug.Log("IsIdle set to false");
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
        ); ;

        if (hit.collider != null)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            isGrounded = (slopeAngle <= maxSlopeAngle) || (slopeAngle >= -maxSlopeAngle);
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector2.up * (isGravityFlipped ? -1 : 1);
        }

        if (isGrounded && isGrabbingWall)
        {
            isGrabbingWall = false;
            Debug.Log("[WallGrab] Автоотцепление от стены при приземлении.");
        }

        if (isGrounded)
        {
            wallGrabCount = 0;
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

        Vector2 dashDirection = new Vector2(groundNormal.y, -groundNormal.x).normalized * facingDirection * (isGravityFlipped ? -1 : 1);
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
    //Отрисовка области проверки земли и стены
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
            Gizmos.DrawLine(playerPos, playerPos + Vector3.left * wallCheckDistance);
            Gizmos.DrawLine(playerPos, playerPos + Vector3.right * wallCheckDistance);

            Gizmos.color = Color.white;
            Handles.Label(playerPos + Vector3.up * 2f, $"Grounded: {isGrounded}\nDashing: {isDashing}\nGrabbingWall: {isGrabbingWall}\nMoveSpeed: {currentMoveSpeed:F2}\nJumps: {JumpCount}");
        }
    }
}