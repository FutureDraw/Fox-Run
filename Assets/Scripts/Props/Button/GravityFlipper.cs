using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    [Header("Объекты управления")]
    public Transform characterVisual;

    [Header("Ground Check")]
    public Transform groundCheck;
    private Vector3 originalGroundCheckLocalPos;
    private Vector3 targetGroundCheckPos;

    [Header("Параметры плавности")]
    [SerializeField] private float flipDuration = 0.3f; // продолжительность плавности переворота

    [Header("Компоненты персонажа")]
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

        if (playerController != null && playerController.groundCheck != null)
        {
            originalGroundCheckLocalPos = playerController.groundCheck.localPosition;
        }
    }

    public void FlipGravity()
    {
        gravityInverted = !gravityInverted;

        // Меняем гравитацию
        Physics2D.gravity = new Vector2(0, gravityInverted ? 9.81f : -9.81f);

        // Определяем целевое положение для groundCheck
        targetGroundCheckPos = originalGroundCheckLocalPos;
        targetGroundCheckPos.y = gravityInverted ? -0.22f : originalGroundCheckLocalPos.y;

        // Инвертируем визуал персонажа
        Vector3 targetScale = characterVisual.localScale;
        targetScale.y *= -1;

        // Инверсия силы прыжка
        if (playerController != null)
        {
            playerController.jumpForce = gravityInverted ? -10f : 10f;
        }

        // Плавное изменение
        StartCoroutine(AnimateFlip(targetScale));
    }

    private System.Collections.IEnumerator AnimateFlip(Vector3 targetScale)
    {
        float elapsedTime = 0f;

        // Плавно анимируем изменения в течение flipDuration
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;

            // Плавно интерполируем изменения для groundCheck и visual
            characterVisual.localScale = Vector3.Lerp(characterVisual.localScale, targetScale, t);
            groundCheck.localPosition = Vector3.Lerp(groundCheck.localPosition, targetGroundCheckPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // На всякий случай зафиксируем конечное состояние
        characterVisual.localScale = targetScale;
        groundCheck.localPosition = targetGroundCheckPos;

        Debug.Log($"Gravity flipped: {(gravityInverted ? "Inverted" : "Normal")}");
    }
}
