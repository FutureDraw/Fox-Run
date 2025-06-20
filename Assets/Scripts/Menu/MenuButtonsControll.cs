﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Управляет кнопками в меню
/// </summary>
public class MenuButtonsControll : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 playPosition;
    public Vector3 savePosition;
    public Vector3 settingPosition;
    public float moveSpeed = 5f;

    public CanvasGroup PlayButtonGroup;
    public CanvasGroup SettingButtonGroup;
    public CanvasGroup ExitButtonGroup;
    public CanvasGroup SavesPanel;
    public CanvasGroup SettingPanel;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private enum PanelState { None, SaveMenu, SettingsMenu }
    private PanelState currentPanel = PanelState.None;

    void Update()
    {
        if (isMoving)
        {
            mainCamera.transform.position = Vector3.MoveTowards(
                mainCamera.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }

        // Новая часть: проверка на нажатие Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPanel == PanelState.SaveMenu || currentPanel == PanelState.SettingsMenu)
            {
                OnBackButtonClicked();
            }
        }
    }

    public void OnPlayButtonClicked()
    {
        targetPosition = savePosition;
        isMoving = true;
        currentPanel = PanelState.SaveMenu;
        StartCoroutine(FadeOut(PlayButtonGroup, 0.3f));
        StartCoroutine(FadeOut(SettingButtonGroup, 0.3f));
        StartCoroutine(FadeOut(ExitButtonGroup, 0.3f));
        StartCoroutine(FadeIn(SavesPanel));
    }

    public void OnSettingButtonClicked()
    {
        targetPosition = settingPosition;
        isMoving = true;
        currentPanel = PanelState.SettingsMenu;
        StartCoroutine(FadeOut(PlayButtonGroup, 0.2f));
        StartCoroutine(FadeOut(SettingButtonGroup, 0.2f));
        StartCoroutine(FadeOut(ExitButtonGroup, 0.2f));
        StartCoroutine(FadeIn(SettingPanel));
    }

    public void OnBackButtonClicked()
    {
        targetPosition = playPosition;
        isMoving = true;

        StartCoroutine(FadeIn(PlayButtonGroup, 0.8f));
        StartCoroutine(FadeIn(SettingButtonGroup, 0.8f));
        StartCoroutine(FadeIn(ExitButtonGroup, 0.8f));

        if (currentPanel == PanelState.SaveMenu)
        {
            StartCoroutine(FadeOut(SavesPanel));
        }
        else if (currentPanel == PanelState.SettingsMenu)
        {
            StartCoroutine(FadeOut(SettingPanel));
        }

        currentPanel = PanelState.None;
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Игра завершена");
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration = 0.5f)
    {
        canvasGroup.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float duration = 0.5f)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsed / duration));
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
