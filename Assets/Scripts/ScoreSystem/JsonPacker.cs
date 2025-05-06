using System;
using UnityEngine;

[Serializable]
public class PlayerRecord
{
    public string playerName;
    public float score;
    public float trophies;
    public string time; // "��:��:fff"
}

public static class JsonPackager
{
    /// <summary>
    /// ��������� ������ � JSON-������.
    /// </summary>
    /// <param name="nick">��� ������.</param>
    /// <param name="score">���������� �����.</param>
    /// <param name="trophies">��������� �����.</param>
    /// <param name="elapsedSeconds">����� ����������� � �������� (float).</param>
    /// <returns>JSON-������ ���� {"playerName":"...","score":...,"trophies":...,"time":"��:��:fff"}</returns>
    public static string Pack(string nick, float score, float trophies, float elapsedSeconds)
    {
        var record = new PlayerRecord
        {
            playerName = nick,
            score = (float)Math.Round((double)score, 2),
            trophies = trophies,
            time = FormatTime(elapsedSeconds)
        };

        // UnityEngine.JsonUtility �������� ������, �� �� ������������ Dictionary � ��������� ���������.
        return JsonUtility.ToJson(record);
    }

    /// <summary>
    /// ����������� ����� ������ � ������ "��:��:fff".
    /// </summary>
    private static string FormatTime(float totalSeconds)
    {
        // �������� TimeSpan ��� �������� ��������������
        TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);
        // mm � ������, ss � �������, fff � ������������
        return string.Format("{0:D2}:{1:D2}:{2:D3}",
                             ts.Minutes,
                             ts.Seconds,
                             ts.Milliseconds);
    }
}
