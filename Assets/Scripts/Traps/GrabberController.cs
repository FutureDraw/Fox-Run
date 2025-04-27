using UnityEngine;

public class GrabberController : MonoBehaviour, ITrap
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;        // Скорость движения grabber'а к цели
    [SerializeField] private int _stopTime = 5;        // Время остановки игрока
    [SerializeField] private float _reachThreshold = 0.1f;  // Порог достижения цели

    [Header("References")]
    [SerializeField] private Transform _targetPoint;   // Целевая точка перемещения

    private Transform _playerTransform;                // Ссылка на трансформ игрока
    private bool _isPulling;                           // Флаг активного перемещения
    private bool _isGrabbed;                           // Флаг захвата игрока
    private PlayerController _playerController;        // Ссылка на контроллер игрока
    private Vector3 _finalPosition;                    // Конечная позиция grabber'а

    private void Start()
    {
        // Настройка коллайдера как триггера
        GetComponent<CircleCollider2D>().isTrigger = true;

        // Получение ссылки на игрока
        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController != null)
        {
            _playerTransform = _playerController.transform;
        }

        Debug.Log("Grabber initialized and armed");
    }

    private void Update()
    {
        // Если grabber движется к цели
        if (_isPulling)
        {
            // Двигаем grabber к цели с заданной скоростью
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPoint.position,
                _speed * Time.deltaTime
            );

            // Проверяем достижение цели
            if (Vector3.Distance(transform.position, _targetPoint.position) < _reachThreshold)
            {
                _isPulling = false;
                _finalPosition = transform.position;  // Запоминаем конечную позицию
            }
        }

        // Если игрок захвачен
        if (_isGrabbed)
        {
            // Привязываем игрока к текущей позиции grabber'а (если движется) или к конечной позиции
            _playerTransform.position = _isPulling ? transform.position : _finalPosition;
        }
    }

    //<Summary>
    // Полная остановка игрока
    //</Summary>
    public void StopPlayer(float time)
    {
        if (_playerController != null)
        {
            _playerController.StopMovement(time);
            Debug.Log($"Player movement frozen for {time} seconds");
        }
    }

    //<Summary>
    // Обработка столкновения с игроком
    //</Summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isGrabbed)
        {
            Debug.Log("Player grabbed");
            _isGrabbed = true;    // Активируем захват
            _isPulling = true;    // Начинаем движение
            StopPlayer(_stopTime); // Останавливаем игрока
            Destroy(gameObject, _stopTime); // Уничтожаем grabber через заданное время
        }
    }

    //<Summary>
    // Нереализовано
    //</Summary>
    public void SlowPlayer(float time, float strength) { }

    //<Summary>
    // Нереализовано
    //</Summary>
    public void KillPlayer() { }
}