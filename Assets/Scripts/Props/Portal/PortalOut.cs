//using UnityEditor.Experimental.GraphView;
using UnityEngine;

// <summary>
// Класс для исходящего портала
// </summary>
public class PortalOut : Portal
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("portal call");
        if (other.CompareTag("Player") && controller != null)
        {
            controller.TeleportPlayer(other.transform, false);
        }
    }
}