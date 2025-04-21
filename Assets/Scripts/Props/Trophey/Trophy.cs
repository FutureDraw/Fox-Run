using UnityEngine;

/// <summary>
/// Класс для описания логики работы одиночного кубка на карте
/// </summary>
public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("trophy collision");
            TrophyController.Instance.CollectTrophy();
            Destroy(gameObject);
        }
    }
}