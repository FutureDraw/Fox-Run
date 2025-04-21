using UnityEngine;

/// <summary>
/// ����� ��� �������� ������ ������ ���������� ����� �� �����
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