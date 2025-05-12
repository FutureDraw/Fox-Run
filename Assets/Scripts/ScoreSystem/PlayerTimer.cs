using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public static PlayerTimer Instance { get; set; }

    private float elapsedTime;
    public float ElapsedTime
    {
        get => elapsedTime;
        set => elapsedTime = value;
    }

    private bool isRunning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        elapsedTime = 0f;
        StopTimer();
        Debug.Log("таймер отключен");
    }
    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log($"Время: {elapsedTime:F2} сек");
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        Debug.Log("Таймер запущен");
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log($"Время прохождения: {elapsedTime:F2} сек");
    }
}
