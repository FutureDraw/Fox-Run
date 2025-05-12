using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Вкладка настроек
/// </summary>
public class SettingsTabs : MonoBehaviour
{
    public GameObject audioPanel;
    public GameObject displayPanel;
    public GameObject controlsPanel;

    public Image audioButtonImage;
    public Image displayButtonImage;
    public Image controlsButtonImage;

    public float activeAlpha = 0.6f;  // Затемнённая кнопка (например, 150/255)
    public float inactiveAlpha = 0f;

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        displayPanel.SetActive(false);
        controlsPanel.SetActive(false);

        UpdateTabTransparency(audioButtonImage);
    }

    public void ShowDisplay()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(true);
        controlsPanel.SetActive(false);

        UpdateTabTransparency(displayButtonImage);
    }

    public void ShowControls()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(false);
        controlsPanel.SetActive(true);

        UpdateTabTransparency(controlsButtonImage);
    }

    private void UpdateTabTransparency(Image activeImage)
    {
        SetAlpha(audioButtonImage, inactiveAlpha);
        SetAlpha(displayButtonImage, inactiveAlpha);
        SetAlpha(controlsButtonImage, inactiveAlpha);

        SetAlpha(activeImage, activeAlpha);
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
