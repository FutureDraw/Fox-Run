using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Класс записок на сцене
/// </summary>
public class SceneNote : MonoBehaviour
{
    [Header("Настройки текста")]
    public GameObject noteCanvasPrefab;
    public string noteText;
    public float textHeightOffset = 2f;
    public float textRightOffset = 1f;
    public float showDuration = 5f;
    public float fadeDuration = 1f;

    [Header("Остановка игрока")]
    public bool stopPlayer = true;
    public float stopDuration = 2f;

    private static GameObject currentNoteCanvas;
    private static Transform currentTarget;
    private RectTransform canvasTransform;
    private bool hasTriggered = false;

    private Collider2D triggerCollider;

    private Animator animator;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            ShowNote(other.transform);
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false; // Отключаем только коллайдер
            }
        }
    }

    private void ShowNote(Transform playerTransform)
    {
        if (currentNoteCanvas != null)
        {
            Destroy(currentNoteCanvas);
        }

        currentTarget = playerTransform;

        // Получаем аниматор и устанавливаем состояние Idle сразу
        animator = playerTransform.GetComponent<Animator>();
            
            
        if (animator != null)
        {
            animator.SetBool("IsIdle", true);
            Debug.Log("IsIdle set to true");
        }

        currentNoteCanvas = Instantiate(noteCanvasPrefab);
        canvasTransform = currentNoteCanvas.GetComponent<RectTransform>();

        TextMeshProUGUI textComponent = currentNoteCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = noteText;
        }

        if (stopPlayer)
        {
            var controller = playerTransform.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.StopMovement(stopDuration);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("IsIdle", false);
                Debug.Log("IsIdle set to false2");
            }
        }

        StartCoroutine(UpdatePosition());
        StartCoroutine(HideNoteAfterDelay(currentNoteCanvas));
    }

    private IEnumerator UpdatePosition()
    {
        while (currentNoteCanvas != null && currentTarget != null)
        {
            Vector3 offset = Vector3.up * textHeightOffset + Vector3.right * textRightOffset;
            currentNoteCanvas.transform.position = currentTarget.position + offset;
            yield return null;
        }
    }

    private IEnumerator HideNoteAfterDelay(GameObject noteObject)
    {
        yield return new WaitForSeconds(showDuration);

        if (noteObject == null) yield break;

        if (fadeDuration > 0f)
        {
            Graphic[] graphics = noteObject.GetComponentsInChildren<Graphic>();
            float t = 0f;
            while (t < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                foreach (Graphic g in graphics)
                {
                    if (g != null)
                    {
                        Color c = g.color;
                        c.a = alpha;
                        g.color = c;
                    }
                }
                t += Time.deltaTime;
                yield return null;
            }
        }

        if (noteObject != null)
        {
            Destroy(noteObject);
            if (noteObject == currentNoteCanvas)
            {
                currentNoteCanvas = null;
                currentTarget = null;
            }
        }
    }
}
