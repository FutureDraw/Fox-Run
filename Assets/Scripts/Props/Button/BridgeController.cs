using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Разводит/опускает мост с процедурной анимацией
/// </summary>
public class BridgeController : MonoBehaviour
{
    public List<ButtonController> buttons;
    [SerializeField] public GameObject AnchorFront;
    [SerializeField] public GameObject AnchorBack;

    [Tooltip("Звук при переключении")]
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    private bool animating = false;
    private bool opening = false;
    private int frameCounter = 0;
    private int totalFrames = 18;
    private float anglePerFrame = 5f;
    private bool isActive = true;
    private bool isCooldown = false;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

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
        if (isCooldown || animating) return; // Блокируем спам

        if (isActive)
            Activate();
        else
            Deactivate();

        isActive = !isActive;
    }

    public void Activate()
    {
        Debug.Log("activate");
        audioSource.PlayOneShot(openSound);
        animating = true;
        opening = true;
        frameCounter = 0;
        StartCoroutine(AnimationCooldown());
    }

    public void Deactivate()
    {
        Debug.Log("deactivate");
        audioSource.PlayOneShot(closeSound);
        animating = true;
        opening = false;
        frameCounter = 0;
        StartCoroutine(AnimationCooldown());
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

    private System.Collections.IEnumerator AnimationCooldown()
    {
        isCooldown = true;
        float frameRate = 1f / 90f;
        float duration = totalFrames * frameRate;
        yield return new WaitForSeconds(duration);
        isCooldown = false;
    }
}
