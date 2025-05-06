using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public static PlayerTimer Instance { get; private set; }

    private float elapsedTime;
    public float ElapsedTime => elapsedTime;
    private bool isRunning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
