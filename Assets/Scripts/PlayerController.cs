using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Проверяем касание снизу
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, whatIsGround);
        isGrounded = false;

        foreach (Collider2D col in colliders)
        {
            // Проверяем, что касание идёт СНИЗУ (а не сбоку или сверху)
            Vector2 contactPoint = col.ClosestPoint(transform.position);
            if (contactPoint.y < transform.position.y - 0.1f)
            {
                isGrounded = true;
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
