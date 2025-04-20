using UnityEngine;

// <summary>
// ������� ����� �������
// </summary>
public abstract class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    public Transform exitPoint; // ����� ������ �� �������
    public PortalController controller;

    // <summary>
    // ��������� ������� ��� ������
    // </summary>
    public Vector2 GetExitPosition()
    {
        return exitPoint != null ? exitPoint.position : transform.position;
    }
}