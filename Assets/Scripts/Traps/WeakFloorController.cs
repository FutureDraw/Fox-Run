using UnityEngine;
using System.Collections;

/// <summary>
/// Управляет проваливающимся полом
/// </summary>
public class WeakFloorController : MonoBehaviour

{
    [SerializeField] public GameObject WeakFloor;
    public float duration;

    [Header("Звук")]
    public AudioClip crackingSound;
    private AudioSource audioSource;


    public void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        Debug.Log("Weak floor initialized and armed");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(crackingSound);
            Destroy(gameObject, duration);
        }
    }
}

