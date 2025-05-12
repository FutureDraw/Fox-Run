using UnityEngine;

/// <summary>
/// Класс для выбора уровня
/// </summary>
public class LevelSelector : MonoBehaviour
{
    public Transform[] levelPositions; // Позиции уровней
    public float moveSpeed = 5f; // Скорость перемещения камеры

    public LevelStatsDisplay statsDisplay;

    private int currentIndex = 0;
    private Transform targetPosition;

    void Start()
    {
        if (levelPositions.Length > 0)
        {
            transform.position = levelPositions[0].position;
            targetPosition = levelPositions[0];
            statsDisplay.LoadLevelStats(1); // первый уровень
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
            statsDisplay.LoadLevelStats(currentIndex + 1); // Индекс уровней с 1
        }
    }

    public void MoveLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            targetPosition = levelPositions[currentIndex];
            statsDisplay.LoadLevelStats(currentIndex + 1); // Индекс уровней с 1
        }
    }
}
