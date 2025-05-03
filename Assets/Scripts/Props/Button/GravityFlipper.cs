using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    [Header("������� ����������")]
    public Camera mainCamera;
    public Transform characterVisual;

    [Header("Ground Check")]
    public Transform groundCheck;
    private Vector3 originalGroundCheckLocalPos;
    private Vector3 targetGroundCheckPos;

    [Header("��������� ���������")]
    [SerializeField] private float flipDuration = 0.3f; // ����������������� ��������� ����������

    [Header("���������� ���������")]
    public PlayerController playerController;

    private Rigidbody2D rb;
    private bool gravityInverted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (characterVisual == null)
            characterVisual = transform;

        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        if (playerController != null && playerController.groundCheck != null)
        {
            originalGroundCheckLocalPos = playerController.groundCheck.localPosition;
        }
    }

    public void FlipGravity()
    {
        gravityInverted = !gravityInverted;

        // ������ ����������
        Physics2D.gravity = new Vector2(0, gravityInverted ? 9.81f : -9.81f);

        // ���������� ������� ��������� ��� groundCheck
        targetGroundCheckPos = originalGroundCheckLocalPos;
        targetGroundCheckPos.y = gravityInverted ? -0.22f : originalGroundCheckLocalPos.y;

        // ������������ ������
        float targetCameraRotation = mainCamera.transform.rotation.eulerAngles.z + 180f;

        // ����������� ������ ���������
        Vector3 targetScale = characterVisual.localScale;
        targetScale.y *= -1;

        // �������� ���� ������
        if (playerController != null)
        {
            playerController.jumpForce = gravityInverted ? -10f : 10f;
        }

        // ������� ���������
        StartCoroutine(AnimateFlip(targetCameraRotation, targetScale));
    }

    private System.Collections.IEnumerator AnimateFlip(float targetCameraRotation, Vector3 targetScale)
    {
        float elapsedTime = 0f;

        // ������ ��������� ��������� � ������� flipDuration
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;

            // ������ ������������� ��������� ��� camera, groundCheck � visual
            mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(mainCamera.transform.rotation.eulerAngles.z, targetCameraRotation, t));
            characterVisual.localScale = Vector3.Lerp(characterVisual.localScale, targetScale, t);
            groundCheck.localPosition = Vector3.Lerp(groundCheck.localPosition, targetGroundCheckPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �� ������ ������ ����������� �������� ���������
        mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, targetCameraRotation);
        characterVisual.localScale = targetScale;
        groundCheck.localPosition = targetGroundCheckPos;

        Debug.Log($"Gravity flipped: {(gravityInverted ? "Inverted" : "Normal")}");
    }
}
