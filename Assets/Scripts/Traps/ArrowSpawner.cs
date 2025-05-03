using UnityEngine;

/// <Summary>
/// Спавнит префаб стрел с заданной скоростью, периодичностью, углом вылета и оффсетом.
/// Отрисовывает в редакторе дебажный вектор направления.
/// </Summary>
public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;      // Префаб стрелы
    [SerializeField] private float _spawnInterval = 1f;    // Период между спавнами
    [SerializeField] private float _arrowSpeed = 5f;       // Скорость полёта
    [SerializeField] private float _arrowLifetime = 5f;    // Время жизни стрелы
    [SerializeField] private float _spawnOffset = 0f;      // Смещение вдоль линии вылета

    [Header("Звук")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Space]
    [SerializeField, Range(0f, 360f)]
    private float _angleDegrees = 0f;                      // Угол вылета (ползунок)

    private float _timer; // Счётчик времени до следующего спавна

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            SpawnArrow();
            _timer = 0f;
        }
    }

    private void SpawnArrow()
    {
        float rad = _angleDegrees * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        // Вычисляем позицию спавна с учётом оффсета
        Vector3 spawnPosition = transform.position + (Vector3)(dir * _spawnOffset);

        GameObject arrowInstance = Instantiate(
            _arrowPrefab,
            spawnPosition,
            Quaternion.Euler(0f, 0f, _angleDegrees - 90f)
        );

        audioSource.PlayOneShot(shootSound);

        var mover = arrowInstance.GetComponent<ArrowMover>();
        if (mover != null)
            mover.Initialize(dir, _arrowSpeed, _arrowLifetime);
    }

    private void OnDrawGizmosSelected()
    {
        float rad = _angleDegrees * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f).normalized;

        Gizmos.color = Color.green;
        Vector3 from = transform.position;
        Vector3 to = from + dir;
        Gizmos.DrawLine(from, to);

        // Стрелка на конце
        Vector3 right = Quaternion.Euler(0, 0, _angleDegrees + 45f) * Vector3.up;
        Vector3 left = Quaternion.Euler(0, 0, _angleDegrees - 225f) * Vector3.up;
        Gizmos.DrawLine(to, to + right * 0.2f);
        Gizmos.DrawLine(to, to + left * 0.2f);

        // Отметка места спавна с учётом оффсета
        Gizmos.color = Color.red;
        Vector3 spawnPoint = from + dir * _spawnOffset;
        Gizmos.DrawSphere(spawnPoint, 0.1f);
    }
}
