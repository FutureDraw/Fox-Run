using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneNote : MonoBehaviour
{
    [TextArea]
    public string noteText = "Твой текст";
    public float displayTime = 5f; // время удаления
    public Canvas noteCanvasPrefab;
    public bool NeedStop = false;
    public float textHeightOffset = 1.5f; // насколько высоко
    public float textRightOffset = 1.0f;  // насколько правее от игрока

    private bool hasActivated = false;
    private void StopPlayer(float time)
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.StopMovement(time);
            Debug.Log($"Player movement frozen for {time} seconds");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;
            if (NeedStop == true)
            {
                StopPlayer(displayTime);
            }
            StartCoroutine(ShowNoteCoroutine(other.transform));
        }

    }

    IEnumerator ShowNoteCoroutine(Transform playerTransform)
    {
        Canvas spawnedCanvas = Instantiate(noteCanvasPrefab);

        Text uiText = spawnedCanvas.GetComponentInChildren<Text>();
        if (uiText != null)
            uiText.text = noteText;

        TextMeshProUGUI tmpText = spawnedCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
            tmpText.text = noteText;

        float timer = 0f;
        while (timer < displayTime)
        {
            if (spawnedCanvas != null && playerTransform != null)
            {
                spawnedCanvas.transform.position = playerTransform.position
                                                   + Vector3.up * textHeightOffset
                                                   + Vector3.right * textRightOffset;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        if (spawnedCanvas != null)
        {
            Destroy(spawnedCanvas.gameObject);
        }
    }
}