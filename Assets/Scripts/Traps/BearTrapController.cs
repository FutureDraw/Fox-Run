using UnityEngine;

public class BearTrap : MonoBehaviour, ITrap
{
    [SerializeField] private int _stopTime = 3;
    [SerializeField] private int _slowTime = 5;
    [SerializeField][Range(0, 1)] private float _slowStrenght = 0.5f;

    [Header("Звук")]
    public AudioClip BearTrapSound;
    private AudioSource audioSource;

    public void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        Debug.Log("Bear trap initialized and armed");
    }

    //<Summary>
    // Полная остановка игрока
    //</Summary>
    public void StopPlayer(float time)
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.StopMovement(time);
            Debug.Log($"Player movement frozen for {time} seconds");
        }
    }

    //<Summary>
    // Замедление игрока
    //</Summary>
    public void SlowPlayer(float time, float strength)
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SlowMovement(time, strength);
            Debug.Log($"Player movement slowed for {time} seconds; strength {strength}");
        }
    }

    //<Summary>
    // Нереализовано
    //</Summary>
    public void KillPlayer()
    { 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(BearTrapSound);
            other.transform.position = transform.position; // Телепортация к центру ловушки
            StopPlayer(_stopTime);
            SlowPlayer(_slowTime, _slowStrenght);
            Destroy(gameObject, _stopTime);
        }
    }
}