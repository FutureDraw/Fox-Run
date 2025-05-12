using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Управляет  Й О Ж И К О М
/// </summary>
public class JogikController : MonoBehaviour
{
    [SerializeField] public GameObject PlaceToTp; // пустой объект к которому переместит игрока после "смерти"
    [SerializeField] AudioSource killSound;
    [SerializeField] public float rotationSpeed = 500;
    public void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        Debug.Log("Jogik initialized and armed");
    }

    public void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }


    ///<Summary>
    /// Не реализовано
    ///</Summary>
    public void StopPlayer(float time)
    {
    }

    ///<Summary>
    /// Не реализовано
    ///</Summary>
    public void SlowPlayer(float time, float strength)
    {
    }

    public void KillPlayer()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = PlaceToTp.transform.position;
            killSound.Play(0);
            Debug.Log($"Player killed >:D");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {  
            KillPlayer();
        }
    }
}