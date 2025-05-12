//using UnityEditor.Experimental.GraphView;
using UnityEngine;


/// <summary>
/// Класс для управления парой порталов
/// </summary>
public class PortalController : MonoBehaviour
{
    [Header("Portal References")]
    public PortalIn portalIn;
    public PortalOut portalOut;

    [Header("Teleport Settings")]
    public float teleportCooldown = 0.5f; // Задержка перед повторным телепортом
    private bool isTeleporting;

    /// <summary>
    /// Инициализация порталов
    /// </summary>
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

        // Назначаем ссылки на контроллер
        portalIn.controller = this;
        portalOut.controller = this;
    }

    /// <summary>
    /// Телепортация игрока между порталами
    /// </summary>
    public void TeleportPlayer(Transform player, bool isEnteringFromIn)
    {
        if (isTeleporting) return;

        isTeleporting = true;

        // Определяем целевой портал
        Portal targetPortal = isEnteringFromIn ? (Portal)portalOut : portalIn;

        // Перемещаем игрока
        player.position = targetPortal.GetExitPosition();

        var player1 = FindObjectOfType<PlayerController>();

        // Запускаем кд телепортации
        Invoke(nameof(ResetTeleport), teleportCooldown);
    }

    /// <summary>
    /// Сброс флага телепортации
    /// </summary>
    private void ResetTeleport()
    {
        isTeleporting = false;
    }
}