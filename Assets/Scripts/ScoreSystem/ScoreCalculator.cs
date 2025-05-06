using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScoreCalculator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ������������� ������ � �������� ��������
        PlayerTimer.Instance.StopTimer();
        float time = PlayerTimer.Instance.ElapsedTime;                // ���.
        float trophies = TrophyController.Instance.TrophiesCollected;   // ��.

        if (trophies == 0)
        {
            trophies = 0.1f;
            return;
        }

        float score = ((trophies * 100) / (time * 1.5f)) * 100f;

        Debug.Log($"=== ���������� ������ ===\n" +
                  $"�����: {time:F2} ���\n" +
                  $"������: {trophies}\n" +
                  $"����: {score:F0}");
    }
}
