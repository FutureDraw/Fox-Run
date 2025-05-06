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
    /// ”паковать данные в JSON-строку.
    /// </summary>
    /// <param name="nick">Ќик игрока.</param>
    /// <param name="score"> оличество очков.</param>
    /// <param name="trophies">—обранные кубки.</param>
    /// <param name="elapsedSeconds">¬рем€ прохождени€ в секундах (float).</param>
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
    /// ‘орматирует число секунд в строку "мм:сс:fff".
    /// </summary>
    private static string FormatTime(float totalSeconds)
    {
        // —оздадим TimeSpan дл€ удобного форматировани€
        TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);
        // mm Ц минуты, ss Ц секунды, fff Ц миллисекунды
        return string.Format("{0:D2}:{1:D2}:{2:D3}",
                             ts.Minutes,
                             ts.Seconds,
                             ts.Milliseconds);
    }
}
