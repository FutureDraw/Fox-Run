using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    [Header("������� ����������")]
    public Transform characterVisual;

    [Header("��������� ���������")]
    [SerializeField] private float flipDuration = 0.3f; // ����������������� ��������� ����������

    [Header("���������� ���������")]
    public PlayerController playerController;

    private Rigidbody2D rb;
    private bool gravityInverted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (characterVisual == null)
            characterVisual = transform;

        if (playerController == null)
            playerController = GetComponent<PlayerController>();

    }

    public void FlipGravity()
    {
        gravityInverted = !gravityInverted;

        // ������ ����������
        Physics2D.gravity = new Vector2(0, gravityInverted ? 9.81f : -9.81f);

        // ����������� ������ ���������
        Vector3 targetScale = characterVisual.localScale;
        targetScale.y *= -1;

        // �������� ���� ������
        if (playerController != null)
        {
            playerController.jumpForce = gravityInverted ? -10f : 10f;
            playerController.isGravityFlipped = gravityInverted;
        }

        // ������� ���������
        StartCoroutine(AnimateFlip(targetScale));
    }

    private System.Collections.IEnumerator AnimateFlip(Vector3 targetScale)
    {
        float elapsedTime = 0f;

        // ������ ��������� ��������� � ������� flipDuration
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;

            // ������ ������������� ��������� ��� visual
            characterVisual.localScale = Vector3.Lerp(characterVisual.localScale, targetScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �� ������ ������ ����������� �������� ���������
        characterVisual.localScale = targetScale;

        Debug.Log($"Gravity flipped: {(gravityInverted ? "Inverted" : "Normal")}");
    }
}
