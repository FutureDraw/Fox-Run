using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public List<ButtonController> buttons;
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
        this.GetComponent<ArrowSpawner>().enabled = true;
        isActive = true;
    }

    public void Deactivate()
    {
        Debug.Log("deactivate");
        this.GetComponent<ArrowSpawner>().enabled = false;
        isActive = false;
    }
}
