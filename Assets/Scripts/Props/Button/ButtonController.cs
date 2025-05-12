using UnityEngine;
using System;

/// <summary>
/// Управляет состоянием кнопки и оповещает подписчиков
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(AudioSource))]
public class ButtonController : MonoBehaviour
{
    [Tooltip("Звук при переключении")]
    public AudioClip pressSound;

    public event Action OnToggle;  // передаёт новое состояние

    private AudioSource audioSource;
    private bool playerNearby = false;
    private PlayerController player;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (playerNearby && player.interaction.action.triggered && player.isGrounded)
            Toggle();
    }

    private void Toggle()
    {
        audioSource.PlayOneShot(pressSound);
        Debug.Log($"{name} pressed");
        OnToggle?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }
}
