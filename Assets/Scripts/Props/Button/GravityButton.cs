using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButton : MonoBehaviour
{
    public List<ButtonController> buttons;
    [SerializeField] private GravityFlipper gravityFlipper;
    private bool isGravityNormal = true;

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
        if (isGravityNormal)
            gravityFlipper.FlipGravity();
        else
            gravityFlipper.FlipGravity();

        isGravityNormal = !isGravityNormal;
    }

}
