using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTimer.Instance.StopTimer();
            // Можно выполнить переход на следующую сцену или показать меню:
            // SceneManager.LoadScene("NextLevel");
        }
    }
}
