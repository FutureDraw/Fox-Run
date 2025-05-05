using UnityEngine;
using UnityEngine.UI;

public class SettingsTabs : MonoBehaviour
{
    public GameObject audioPanel;
    public GameObject displayPanel;
    public GameObject controlsPanel;

    public void OnClickSettingsButton()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        displayPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void ShowDisplay()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        audioPanel.SetActive(false);
        displayPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
}
