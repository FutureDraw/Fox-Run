using UnityEngine;
using TMPro;
using System.IO;

public class LevelStatsDisplay : MonoBehaviour
{
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

        // Активируем только панель текущего уровня
        for (int i = 0; i < levelUis.Length; i++)
        {
            levelUis[i].panel.SetActive(i == levelIndex - 1);
        }

        if (levelIndex < 1 || levelIndex > levelUis.Length)
        {
            Debug.LogWarning("Недопустимый индекс уровня: " + levelIndex);
            return;
        }

        var ui = levelUis[levelIndex - 1];

        string path = Path.Combine(Application.persistentDataPath, $"level_{levelIndex}.json");

        // Если файл не существует, выводим дефолтные данные
        if (!File.Exists(path))
        {
            Debug.Log("Файл не найден, показываю дефолтные значения");
            ui.passedText.text = "Нет";
            ui.bestTimeText.text = "—";
            ui.trophiesText.text = "0";

            // Цвет для не пройденного уровня (красный)
            ui.passedText.color = Color.red;
            ui.bestTimeText.color = Color.red;
            ui.trophiesText.color = Color.red;
            return;
        }

        // Чтение и десериализация данных
        string json = File.ReadAllText(path);
        PlayerRecord record = JsonUtility.FromJson<PlayerRecord>(json);

        // Проверяем, пройден ли уровень
        if (!string.IsNullOrEmpty(record.time) && record.trophies > 0)
        {
            ui.passedText.text = "Да";
            ui.passedText.color = Color.green;

            // Если время меньше, обновляем его
            if (float.Parse(record.time.Replace(":", "").Replace(".", "")) < float.Parse(ui.bestTimeText.text.Replace(":", "").Replace(".", "")))
            {
                ui.bestTimeText.text = record.time;
            }

            // Если трофеев больше, обновляем значение
            if (record.trophies > int.Parse(ui.trophiesText.text))
            {
                ui.trophiesText.text = $"{record.trophies}";
            }

            ui.bestTimeText.color = Color.green;
            ui.trophiesText.color = Color.green;
        }
        else
        {
            // Если не пройдено, делаем всё красным
            ui.passedText.text = "Нет";
            ui.bestTimeText.text = "—";
            ui.trophiesText.text = "0";

            ui.passedText.color = Color.red;
            ui.bestTimeText.color = Color.red;
            ui.trophiesText.color = Color.red;
        }
    }
}
