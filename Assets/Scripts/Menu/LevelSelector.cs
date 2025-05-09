using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public Transform[] levelPositions; // ������� �������
    public float moveSpeed = 5f; // �������� ����������� ������

    public LevelStatsDisplay statsDisplay;

    private int currentIndex = 0;
    private Transform targetPosition;

    void Start()
    {
        if (levelPositions.Length > 0)
        {
            transform.position = levelPositions[0].position;
            targetPosition = levelPositions[0];
            statsDisplay.LoadLevelStats(1); // ������ �������
        }
    }

    void Update()
    {
        if (targetPosition != null && transform.position != targetPosition.position)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, Time.deltaTime * moveSpeed);
        }
    }

    public void MoveRight()
    {
        if (currentIndex < levelPositions.Length - 1)
        {
            currentIndex++;
            targetPosition = levelPositions[currentIndex];
            statsDisplay.LoadLevelStats(currentIndex + 1); // ������ ������� � 1
        }
    }

    public void MoveLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            targetPosition = levelPositions[currentIndex];
            statsDisplay.LoadLevelStats(currentIndex + 1); // ������ ������� � 1
        }
    }
}
