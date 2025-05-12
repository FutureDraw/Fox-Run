using UnityEngine;

/// <summary>
/// Класс для описания работы системы кубков
/// </summary>
public class TrophyController : MonoBehaviour
{
    public static TrophyController Instance { get; private set; }
    private int trophiesCollected = 0;
    public int TrophiesCollected => trophiesCollected;

    [Header("Звук")]
    public AudioClip collectSound;
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CollectTrophy()
    {
        trophiesCollected++;
        audioSource.PlayOneShot(collectSound);
        Debug.Log($"Кубков собрано: {trophiesCollected}");
    }
}