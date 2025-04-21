using UnityEngine;

/// <summary>
/// ����� ��� �������� ������ ������� ������
/// </summary>
public class TrophyController : MonoBehaviour
{
    public static TrophyController Instance { get; private set; }
    private int trophiesCollected = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CollectTrophy()
    {
        trophiesCollected++;
        Debug.Log($"������ �������: {trophiesCollected}");
    }
}