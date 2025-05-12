//using UnityEditor.Experimental.GraphView;
using UnityEngine;

// <summary>
// Класс для входящего портала
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