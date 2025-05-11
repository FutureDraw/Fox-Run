using UnityEngine;
using System.IO;

public class ScoreCalculator : MonoBehaviour
{
    public int levelIndex = 1; // ���������� � ����������

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
        float time = PlayerTimer.Instance.ElapsedTime;                  // ���.
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

        PlayerRecord record = new PlayerRecord
        {
            playerName = nick,
            score = score,
            trophies = trophies,
            timeInSeconds = time
        };

        string jsonPayload = JsonUtility.ToJson(record, true);
        Debug.Log("JSON ��� ����������: " + jsonPayload);

        string path = Path.Combine(Application.persistentDataPath, $"level_{levelIndex}.json");

        if (File.Exists(path))
        {
            // ������ ������� ��������� �� �����
            string currentJson = File.ReadAllText(path);
            PlayerRecord currentRecord = JsonUtility.FromJson<PlayerRecord>(currentJson);

            // ���������� �����: ���� ����� ����� ������ ��������, ��������� ����� ���������
            if (time < currentRecord.timeInSeconds)
            {
                File.WriteAllText(path, jsonPayload);
                Debug.Log("����� ��������� ������� �� ����: " + path);
            }
            else
            {
                Debug.Log("����� ����� ������ ��� ����� ��������, ��������� �� �������.");
            }
        }
        else
        {
            // ���� ����� ��� ���, ������ ��� � ��������� ���������
            File.WriteAllText(path, jsonPayload);
            Debug.Log("���� ������� �� ����: " + path);
        }
    }
}
