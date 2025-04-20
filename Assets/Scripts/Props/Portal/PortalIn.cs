using UnityEditor.Experimental.GraphView;
using UnityEngine;

// <summary>
// ����� ��� ��������� �������
// </summary>
public class PortalIn : Portal
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("portal call");
        if (other.CompareTag("Player") && controller != null)
        {
            controller.TeleportPlayer(other.transform, true);
        }
    }
}