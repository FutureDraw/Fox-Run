using UnityEngine;
using System.IO;

public class ScoreCalculator : MonoBehaviour
{
    public int levelIndex = 1; // установить в инспекторе

    [System.Serializable]
    public class PlayerRecord
    {
        public string playerName;
        public float score;
        public float trophies;
        public float timeInSeconds;
    }

    public void CalculateResult()
    {
        string nick = "nickname"; // placeholder
        float time = PlayerTimer.Instance.ElapsedTime;                  // сек.
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

        PlayerRecord record = new PlayerRecord
        {
            playerName = nick,
            score = score,
            trophies = trophies,
            timeInSeconds = time
        };

        string jsonPayload = JsonUtility.ToJson(record, true);
        Debug.Log("JSON для сохранения: " + jsonPayload);

        string path = Path.Combine(Application.persistentDataPath, $"level_{levelIndex}.json");

        if (File.Exists(path))
        {
            // Читаем текущий результат из файла
            string currentJson = File.ReadAllText(path);
            PlayerRecord currentRecord = JsonUtility.FromJson<PlayerRecord>(currentJson);

            // Сравниваем время: если новое время меньше текущего, сохраняем новый результат
            if (time < currentRecord.timeInSeconds)
            {
                File.WriteAllText(path, jsonPayload);
                Debug.Log("Новый результат сохранён по пути: " + path);
            }
            else
            {
                Debug.Log("Новое время больше или равно текущему, результат не сохранён.");
            }
        }
        else
        {
            // Если файла ещё нет, создаём его и сохраняем результат
            File.WriteAllText(path, jsonPayload);
            Debug.Log("Файл сохранён по пути: " + path);
        }
    }
}
