using UnityEngine;

// <summary>
// Базовый класс портала
// </summary>
public abstract class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    public Transform exitPoint; // Точка выхода из портала
    public PortalController controller;

    // <summary>
    // Получение позиции для выхода
    // </summary>
    public Vector2 GetExitPosition()
    {
        return exitPoint != null ? exitPoint.position : transform.position;
    }
}