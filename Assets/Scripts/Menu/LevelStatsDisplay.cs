using UnityEngine;
using TMPro;
using System.IO;

public class LevelStatsDisplay : MonoBehaviour
{
    [System.Serializable]
    public class PlayerRecord
    {
        public string playerName;
        public float score;
        public float trophies;
        public float timeInSeconds; // Время в секундах
    }

    [System.Serializable]
    public class LevelUI
    {
        public GameObject panel; // Вся панель уровня
        public TextMeshProUGUI passedText;
        public TextMeshProUGUI bestTimeText;
        public TextMeshProUGUI trophiesText;
    }

    public LevelUI[] levelUis; // Массив панелей под каждый уровень

    public void LoadLevelStats(int levelIndex)
    {
        Debug.Log("Загрузка статистики для уровня: " + levelIndex);

        for (int i = 0; i < levelUis.Length; i++)
            levelUis[i].panel.SetActive(i == levelIndex - 1);

        if (levelIndex < 1 || levelIndex > levelUis.Length)
        {
            Debug.LogWarning("Недопустимый индекс уровня: " + levelIndex);
            return;
        }

        var ui = levelUis[levelIndex - 1];
        string path = Path.Combine(Application.persistentDataPath, $"level_{levelIndex}.json");

        Debug.Log("Путь к JSON: " + path);

        if (!File.Exists(path))
        {
            Debug.Log("Файл не найден, показываю дефолтные значения");
            SetDefaultUI(ui);
            return;
        }

        string json = File.ReadAllText(path);
        PlayerRecord record = JsonUtility.FromJson<PlayerRecord>(json);

        Debug.Log("данные:" + record + "трофеи: " + record.trophies + "время:" + record.timeInSeconds);

        if (record != null && record.trophies > 0 && record.timeInSeconds > 0)
        {
            ui.passedText.text = "Да";
            ui.passedText.color = Color.green;

            float currentDisplayedTime = ParseTime(ui.bestTimeText.text);
            if (currentDisplayedTime <= 0 || record.timeInSeconds < currentDisplayedTime)
            {
                ui.bestTimeText.text = FormatTime(record.timeInSeconds);
            }

            float currentTrophies = 0;
            float.TryParse(ui.trophiesText.text, out currentTrophies);
            if (record.trophies > currentTrophies)
            {
                ui.trophiesText.text = record.trophies.ToString("0");
            }

            ui.bestTimeText.color = Color.green;
            ui.trophiesText.color = Color.green;
        }
        else
        {
            Debug.Log("Не удалось загрузить данные");
            SetDefaultUI(ui);
        }
    }

    private void SetDefaultUI(LevelUI ui)
    {
        ui.passedText.text = "Нет";
        ui.bestTimeText.text = "—";
        ui.trophiesText.text = "0";

        ui.passedText.color = Color.red;
        ui.bestTimeText.color = Color.red;
        ui.trophiesText.color = Color.red;
    }

    private float ParseTime(string timeText)
    {
        if (string.IsNullOrWhiteSpace(timeText) || timeText == "—")
            return -1;

        string[] parts = timeText.Split(':');
        if (parts.Length != 3) return -1;

        float minutes = float.Parse(parts[0]);
        float seconds = float.Parse(parts[1]);
        float milliseconds = float.Parse(parts[2]);

        return minutes * 60f + seconds + milliseconds / 1000f;
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
