using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScoreCalculator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Останавливаем таймер и получаем значения
        PlayerTimer.Instance.StopTimer();
        float time = PlayerTimer.Instance.ElapsedTime;                // сек.
        float trophies = TrophyController.Instance.TrophiesCollected;   // шт.

        if (trophies == 0)
        {
            trophies = 0.1f;
            return;
        }

        float score = ((trophies * 100) / (time * 1.5f)) * 100f;

        Debug.Log($"=== Результаты уровня ===\n" +
                  $"Время: {time:F2} сек\n" +
                  $"Кубков: {trophies}\n" +
                  $"Очки: {score:F0}");
    }
}
