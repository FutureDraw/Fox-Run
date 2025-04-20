using UnityEngine;

public class GrabberController : MonoBehaviour, ITrap
{
    [SerializeField] private int _stopTime = 5;
    [SerializeField] private bool _grabbed = false;
    public Transform _playerTransform;


    public void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        Debug.Log("Grabber initialized and armed");
    }

    public void Update()
    {
        if (_grabbed)
        {
            _playerTransform.position = transform.position;
            Debug.Log("grabbed");
        }
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
    // Нереализовано
    //</Summary>
    public void SlowPlayer(float time, float strength)
    {
    }

    //<Summary>
    // Нереализовано
    //</Summary>
    public void KillPlayer()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collision");
            _grabbed = false;
            StopPlayer(_stopTime);
            _grabbed = true;
            Destroy(gameObject, _stopTime + 0.2f);
        }
    }
}