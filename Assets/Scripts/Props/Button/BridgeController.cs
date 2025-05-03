using System.Collections.Generic;
using UnityEngine;

// <summary>
// Разводит/опускает мост с процедурной анимацией
// </summary>
public class BridgeController : MonoBehaviour
{
    public List<ButtonController> buttons;
    [SerializeField] public GameObject AnchorFront;
    [SerializeField] public GameObject AnchorBack;


    private bool animating = false;
    private bool opening = false;
    private int frameCounter = 0;
    private int totalFrames = 18;
    private float anglePerFrame = 5f;
    bool isActive = true;

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
        
        if (isActive) Activate();
        else Deactivate();
        isActive = !isActive;
    }

    public void Activate()
    {
        Debug.Log("activate");
        if (!animating)
        {
            animating = true;
            opening = true;
            frameCounter = 0;
        }
    }

    public void Deactivate()
    {
        Debug.Log("deactivate");
        if (!animating)
        {
            animating = true;
            opening = false;
            frameCounter = 0;
        }
    }

    private void Update()
    {
        if (!animating) return;

        if (frameCounter < totalFrames)
        {
            float angle = opening ? -anglePerFrame : anglePerFrame;
            AnchorFront.transform.Rotate(Vector3.forward, angle);
            AnchorBack.transform.Rotate(Vector3.forward, -angle);
            frameCounter++;
        }
        else
        {
            animating = false;
        }
    }
}
