using UnityEngine;

/// <summary>
/// ����� ��� �������� ��������� ������� ������ ���������
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;


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

        // ���������� ����������� �������
        if (moveX != 0)
            facingDirection = (int)Mathf.Sign(moveX);

        GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
            }
            //������ ������
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
    }
    // <summary>
    // �������� �� ������������ � ������
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

    /// <summary>
    /// Debug ��������� �������
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
