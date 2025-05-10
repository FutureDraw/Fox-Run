using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour

{
    public StartTrigger startTrigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTimer.Instance.StopTimer();
            startTrigger.GetComponent<Collider2D>().enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("LevelSelector");
        }
    }
}
