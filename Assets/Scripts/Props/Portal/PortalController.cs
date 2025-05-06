//using UnityEditor.Experimental.GraphView;
using UnityEngine;


// <summary>
// ����� ��� ���������� ����� ��������
// </summary>
public class PortalController : MonoBehaviour
{
    [Header("Portal References")]
    public PortalIn portalIn;
    public PortalOut portalOut;

    [Header("Teleport Settings")]
    public float teleportCooldown = 0.5f; // �������� ����� ��������� ����������
    private bool isTeleporting;

    // <summary>
    // ������������� ��������
    // </summary>
    private void Start()
    {
        if (portalIn == null || portalOut == null)
        {
            Debug.LogError("Portals not assigned in PortalController");
            return;
        }
        else 
        {
            Debug.Log("Portals initialized");
        }

        // ��������� ������ �� ����������
        portalIn.controller = this;
        portalOut.controller = this;
    }

    // <summary>
    // ������������ ������ ����� ���������
    // </summary>
    public void TeleportPlayer(Transform player, bool isEnteringFromIn)
    {
        if (isTeleporting) return;

        isTeleporting = true;

        // ���������� ������� ������
        Portal targetPortal = isEnteringFromIn ? (Portal)portalOut : portalIn;

        // ���������� ������
        player.position = targetPortal.GetExitPosition();

        var player1 = FindObjectOfType<PlayerController>();

        // ��������� �� ������������
        Invoke(nameof(ResetTeleport), teleportCooldown);
    }

    // <summary>
    // ����� ����� ������������
    // </summary>
    private void ResetTeleport()
    {
        isTeleporting = false;
    }
}