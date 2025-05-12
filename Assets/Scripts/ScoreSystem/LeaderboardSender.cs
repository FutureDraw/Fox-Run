using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class LeaderboardSender : MonoBehaviour
{
    private const string url = "http://138.124.99.121:5000/submit_score";

    public void Submit(string nick, float score, float trophies, float elapsedSeconds)
    {
        string json = JsonPackager.Pack(nick, score, trophies, elapsedSeconds);
        StartCoroutine(Send(json));
    }


    private IEnumerator Send(string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Данные отправлены");
        else
            Debug.LogError("Ошибка отправки: " + request.error);
    }
}
