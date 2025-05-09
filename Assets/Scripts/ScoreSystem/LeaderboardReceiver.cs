using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LeaderboardReceiver : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;
    private const string url = "http://138.124.99.121:5000/get_leaderboard";

    public void LoadLeaderboard()
    {
        StartCoroutine(GetLeaderboard());
    }

    private IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"records\":" + request.downloadHandler.text + "}";
            PlayerRecordList recordList = JsonUtility.FromJson<PlayerRecordList>(json);
            Display(recordList.records);
        }
        else
        {
            Debug.LogError("Ошибка загрузки таблицы лидеров: " + request.error);
        }
    }

    private void Display(PlayerRecord[] records)
    {
        leaderboardText.text = "";
        foreach (var rec in records)
        {
            leaderboardText.text += $"{rec.playerName} - {rec.score} - {rec.time} - {rec.trophies}\n";
        }
    }

    [System.Serializable]
    public class PlayerRecordList
    {
        public PlayerRecord[] records;
    }
}
