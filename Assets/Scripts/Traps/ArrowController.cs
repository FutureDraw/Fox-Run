using UnityEngine;

public class Arrow : MonoBehaviour, ITrap
{
    [SerializeField] private int _slowTime = 2;
    [SerializeField][Range(0, 1)] private float _slowStrenght = 0.3f;

    public void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        Debug.Log("Arrow initialized and armed");
    }

    //<Summary>
    // Полная остановка игрока
    //</Summary>
    public void StopPlayer(float time)
    {
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
        // Не уничтожаем стрелу если это триггер
        //if (other.isTrigger) return;
        Debug.Log("Col");
        if (other.CompareTag("Player"))
        {
            SlowPlayer(_slowTime, _slowStrenght);
        }

        Destroy(gameObject, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0f);
    }
}