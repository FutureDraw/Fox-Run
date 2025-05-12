using System.Collections;
using UnityEngine;

/// <summary>
/// Инициализатор скриптов
/// </summary>
public class Bindinit : MonoBehaviour
{
    public GameObject settingsPanel;
    private RectTransform panelRectTransform;
    public float displayDuration = 0.005f;

    void Start()
    {
        panelRectTransform = settingsPanel.GetComponent<RectTransform>();
        StartCoroutine(InitializeMenu());
    }

    private IEnumerator InitializeMenu()
    {
        yield return new WaitForSeconds(0.1f);

        settingsPanel.SetActive(true);
        panelRectTransform.anchoredPosition = new Vector2(0, 1000);

        yield return new WaitForSeconds(displayDuration);

        settingsPanel.SetActive(false);
        panelRectTransform.anchoredPosition = new Vector2(0, 0);
    }
}
