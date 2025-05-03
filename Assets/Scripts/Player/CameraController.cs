using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Tracking")]
    public Transform target;                 // Объект, за которым следует камера
    public Vector2 horizontalOffset = new Vector2(2f, 0.3f);  // Смещение камеры по X (мин, макс)
    public float smoothTime = 0.3f;          // Время сглаживания движения камеры

    [Header("Vertical Tracking")]
    [Range(0f, 1f)]
    public float viewportTargetY = 0.45f;    // Желаемая позиция игрока по вертикали в Viewport
    private float lowerTertie = 1f / 3f;
    private float upperTertie = 2f / 3f;
    public float baseVerticalSpeed = 10f;    // Базовая скорость слежения по вертикали
    public float maxVerticalSpeed = 1000f;   // Максимальная скорость слежения по вертикали

    [Header("Zoom")]
    public float zoomSpeed = 3f;             // Скорость изменения зума
    public float minZoom = 4f;               // Минимальный зум
    public float maxZoom = 10f;              // Максимальный зум
    public float targetZoom;                 // Целевой зум камеры

    [Header("Camera Lock")]
    public bool isLocked;                    // Флаг блокировки камеры
    public Vector3 lockedPosition;           // Позиция при блокировке камеры

    private Camera cam;                      // Компонент камеры
    private Vector3 velocity = Vector3.zero; // Вектор скорости для SmoothDamp по горизонтали
    private float verticalVelocity = 0f;     // Скорость для сглаживания по вертикали

    void Start()
    {
        // Инициализация компонентов
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoomInput();
    }

    void LateUpdate()
    {
        // Выбор поведения камеры в зависимости от состояния блокировки
        if (isLocked)
            LockCameraBehavior();
        else
            FollowPlayerBehavior();
    }

    //<summary>
    // Обрабатывает ввод для изменения зума камеры
    //</summary>
    void HandleZoomInput()
    {
        if (Input.GetKey(KeyCode.Equals)) targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Minus)) targetZoom += zoomSpeed * Time.deltaTime;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 5f);
    }

    //<summary>
    // Поведение камеры при следовании за игроком
    //</summary>
    void FollowPlayerBehavior()
    {
        if (target == null) return;

        // Горизонтальное смещение
        float xOffset = Mathf.Lerp(
            horizontalOffset.x,
            horizontalOffset.y,
            Mathf.Abs(target.localScale.x)
        );
        float targetX = target.position.x + xOffset;

        // Вертикальное поведение
        float viewportY = cam.WorldToViewportPoint(target.position).y;
        float targetY = transform.position.y;

        if (viewportY < lowerTertie || viewportY > upperTertie)
        {
            // Игрок вышел за пределы третей - двигаем камеру
            float idealCamY = target.position.y - (viewportTargetY - 0.5f) * (2f * cam.orthographicSize);

            // Вычисляем нормализованное отклонение
            float deviation = viewportY > upperTertie
                ? (viewportY - upperTertie) / (1f - upperTertie)
                : (lowerTertie - viewportY) / lowerTertie;
            deviation = Mathf.Clamp01(deviation);

            // Скорость слежения
            float followSpeed = Mathf.Lerp(baseVerticalSpeed, maxVerticalSpeed, deviation);

            // Плавное сглаживание по вертикали
            float newY = Mathf.SmoothDamp(
                transform.position.y,
                idealCamY,
                ref verticalVelocity,
                1f / followSpeed
            );
            targetY = newY;
        }
        // иначе внутри третей - оставляем текущую высоту

        Vector3 desiredPos = new Vector3(targetX, targetY, transform.position.z);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            smoothTime
        );
    }

    //<summary>
    // Поведение камеры в заблокированном состоянии
    //</summary>
    void LockCameraBehavior()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            lockedPosition,
            ref velocity,
            smoothTime
        );
    }

    // API для внешнего контроля

    //<summary>
    // Блокирует камеру в указанной позиции
    //</summary>
    //<param name="position">Позиция для блокировки камеры</param>
    public void SetCameraLock(Vector3 position)
    {
        isLocked = true;
        lockedPosition = position;
    }

    //<summary>
    // Разблокирует камеру
    //</summary>
    public void ReleaseCameraLock()
    {
        isLocked = false;
    }

    //<summary>
    // Устанавливает целевой зум камеры
    //</summary>
    //<param name="zoom">Значение зума (будет ограничено minZoom/maxZoom)</param>
    public void SetZoom(float zoom)
    {
        targetZoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    //<summary>
    // Устанавливает плавность движения камеры
    //</summary>
    //<param name="value">Новое значение smoothTime</param>
    public void SetSmoothness(float value) => smoothTime = value;

    //<summary>
    // Запуск катсцены
    //</summary>
    public void PlayCutscene(Vector3 position, float zoom, float smoothness = 0.3f)
    {
        SetCameraLock(position);
        SetZoom(zoom);
        SetSmoothness(smoothness);
    }

    //<summary>
    // Выход из катсцены
    //</summary>
    public void EndCutscene()
    {
        ReleaseCameraLock();
    }
}
