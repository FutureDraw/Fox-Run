using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrapController : MonoBehaviour
{
    [SerializeField] public GameObject AnchorFront;
    [SerializeField] public GameObject AnchorBack;
    private int frameCounter = 0;
    private bool isWorked = false;
    

    public void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        Debug.Log("Door trap initialized and armed");
    }

    private void Update()
    {
        if (frameCounter < 18 && isWorked == true)
        {
            AnchorFront.transform.Rotate(Vector3.forward, -5f);  //ÏÐÎÖÅÄÓÐÍÀß ÀÍÈÌÀÖÈß ÀÀÀÀÀÀÀÀÀÀÀÀÀÀ
            AnchorBack.transform.Rotate(Vector3.forward, 5f);
            frameCounter++;
        }
    }

    //<Summary>
    //
    //</Summary>

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            isWorked = true;
        }
    }
}
