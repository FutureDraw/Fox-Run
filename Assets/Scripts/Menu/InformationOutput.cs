using UnityEngine;
using TMPro;

/// <summary>
/// Вывод информации в селектор уровней
/// </summary>
public class InformationOutput : MonoBehaviour
{
    public TextMeshProUGUI m_CollectedTrophies;
    public TextMeshProUGUI m_Timer;

    void Update()
    {
        if (TrophyController.Instance != null)
        {
            int trophies = TrophyController.Instance.TrophiesCollected;
            m_CollectedTrophies.text = $"{trophies}";
        }

        if (PlayerTimer.Instance != null)
        {
            float time = PlayerTimer.Instance.ElapsedTime;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            m_Timer.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
