using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTimer.Instance.StartTimer();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
