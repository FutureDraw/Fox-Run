using UnityEngine;
using System.Collections;

public class WeakFloorController : MonoBehaviour

{
    [SerializeField] public GameObject WeakFloor;
    private bool isWorked = false;
    public float duration;


    public void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        Debug.Log("Weak floor initialized and armed");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject, duration);
        }
    }
}

