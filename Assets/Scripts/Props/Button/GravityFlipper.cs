using UnityEngine;

/// <summary>
/// Управляет состоянием гравитации
/// </summary>
public class GravityFlipper : MonoBehaviour
{
    [Header("Объекты управления")]
    public Transform characterVisual;

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

    }

    public void FlipGravity()
    {
        gravityInverted = !gravityInverted;

        // Меняем гравитацию
        Physics2D.gravity = new Vector2(0, gravityInverted ? 9.81f : -9.81f);

        // Инвертируем визуал персонажа
        Vector3 targetScale = characterVisual.localScale;
        targetScale.y *= -1;

        // Инверсия силы прыжка
        if (playerController != null)
        {
            playerController.jumpForce = gravityInverted ? -10f : 10f;
            playerController.isGravityFlipped = gravityInverted;
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

            // Плавно интерполируем изменения для visual
            characterVisual.localScale = Vector3.Lerp(characterVisual.localScale, targetScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // На всякий случай зафиксируем конечное состояние
        characterVisual.localScale = targetScale;

        Debug.Log($"Gravity flipped: {(gravityInverted ? "Inverted" : "Normal")}");
    }
}
