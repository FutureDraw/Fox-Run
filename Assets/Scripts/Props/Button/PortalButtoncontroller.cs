using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Включает/выключает порталы
/// </summary>
public class PortalButtoncontroller : MonoBehaviour
{
    public List<ButtonController> buttons;
    [SerializeField] public GameObject Portal1;
    [SerializeField] public GameObject Portal2;

    private bool isActive = true;

    private void OnEnable()
    {
        foreach (var btn in buttons)
            btn.OnToggle += HandleToggle;
    }

    private void OnDisable()
    {
        foreach (var btn in buttons)
            btn.OnToggle -= HandleToggle;
    }

    private void HandleToggle()
    {
        if (!isActive) Activate();
        else Deactivate();
    }

    public void Activate()
    {
        Debug.Log("activate");
        Portal1.SetActive(true);
        Portal2.SetActive(true);
        isActive = true;
    }

    public void Deactivate()
    {
        Debug.Log("deactivate");
        Portal1.SetActive(false);
        Portal2.SetActive(false);
        isActive = false;
    }
}
