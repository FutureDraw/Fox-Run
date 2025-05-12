using System;
using UnityEngine;

[Serializable]
public class PlayerRecord
{
    public string playerName;
    public float score;
    public float trophies;
    public string time; // "мм:сс:fff"
}

public static class JsonPackager
{
    /// <summary>
    /// Упаковать данные в JSON-строку.
    /// </summary>
    /// <param name="nick">Ник игрока.</param>
    /// <param name="score">Количество очков.</param>
    /// <param name="trophies">Собранные кубки.</param>
    /// <param name="elapsedSeconds">Время прохождения в секундах (float).</param>
    /// <returns>JSON-строка вида {"playerName":"...","score":...,"trophies":...,"time":"мм:сс:fff"}</returns>
    public static string Pack(string nick, float score, float trophies, float elapsedSeconds)
    {
        var record = new PlayerRecord
        {
            playerName = nick,
            score = (float)Math.Round((double)score, 2),
            trophies = trophies,
            time = FormatTime(elapsedSeconds)
        };

        // UnityEngine.JsonUtility работает быстро, но не поддерживает Dictionary и некоторые коллекции.
        return JsonUtility.ToJson(record);
    }

    /// <summary>
    /// Форматирует число секунд в строку "мм:сс:fff".
    /// </summary>
    private static string FormatTime(float totalSeconds)
    {
        // Создадим TimeSpan для удобного форматирования
        TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);
        // mm – минуты, ss – секунды, fff – миллисекунды
        return string.Format("{0:D2}:{1:D2}:{2:D3}",
                             ts.Minutes,
                             ts.Seconds,
                             ts.Milliseconds);
    }
}
