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
            Debug.Log($"�����: {elapsedTime:F2} ���");
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        Debug.Log("������ �������");
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log($"����� �����������: {elapsedTime:F2} ���");
    }
}
