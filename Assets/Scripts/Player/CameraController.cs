using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Tracking")]
    public Transform target;                // Объект, за которым следует камера
    public Vector2 horizontalOffset = new Vector2(2f, 0.3f);  // Смещение камеры по X (мин, макс)
    public float smoothTime = 0.3f;          // Время сглаживания движения камеры

    [Header("Height Levels")]
    public float[] heightLevels;             // Уровни высот для камеры
    public float heightChangeSpeed = 2f;     // Скорость изменения высоты камеры

    [Header("Zoom")]
    public float zoomSpeed = 3f;             // Скорость изменения зума
    public float minZoom = 4f;               // Минимальный зум
    public float maxZoom = 10f;              // Максимальный зум

    [Header("Camera Lock")]
    public bool isLocked;                    // Флаг блокировки камеры
    public Vector3 lockedPosition;           // Позиция при блокировке камеры

    private Camera cam;                      // Компонент камеры
    private Vector3 velocity = Vector3.zero; // Вектор скорости для SmoothDamp
    private float currentHeight;             // Текущая высота камеры
    public float targetZoom;                 // Целевой зум камеры
    private bool isHeightTransition;         // Флаг перехода между уровнями высот
    private bool isFollowingDueToExit;       // Флаг для отслеживания выхода игрока за границы камеры

    void Start()
    {
        // Инициализация компонентов
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
        currentHeight = transform.position.y;
    }

    void Update()
    {
        HandleZoomInput();
    }

    void LateUpdate()
    {
        // Выбор поведения камеры в зависимости от состояния блокировки
        if (isLocked)
        {
            LockCameraBehavior();
        }
        else
        {
            FollowPlayerBehavior();
        }
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

        var playerController = target.GetComponent<PlayerController>();
        var targetPos = CalculateCameraPosition(playerController);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }

    //<summary>
    // Вычисляет целевую позицию камеры на основе позиции игрока
    //</summary>
    //<param name="player">Контроллер игрока для проверки состояния</param>
    //<returns>Вектор целевой позиции камеры</returns>
    Vector3 CalculateCameraPosition(PlayerController player)
    {
        // Вычисляем смещение по X в зависимости от масштаба цели
        float xOffset = Mathf.Lerp(
            horizontalOffset.x,
            horizontalOffset.y,
            Mathf.Abs(target.localScale.x)
        );

        float targetX = target.position.x + xOffset;
        float targetY = currentHeight;

        // Если игрок на земле - меняем высоту камеры
        if (player.isGrounded)
        {
            var newHeight = GetHeightLevel();
            if (!isHeightTransition)
            {
                targetY = newHeight;
                isHeightTransition = true;
            }
            currentHeight = Mathf.Lerp(currentHeight, newHeight, Time.deltaTime * heightChangeSpeed);
        }
        else
        {
            isHeightTransition = false;

            // Проверяем, вышел ли игрок за границы камеры
            if (IsPlayerOutOfCameraView())
            {
                isFollowingDueToExit = true; // Активируем режим следования
            }

            // Если игрок вышел за границы - следуем за ним по вертикали
            if (isFollowingDueToExit)
            {
                targetY = target.position.y;
                currentHeight = Mathf.Lerp(currentHeight, targetY, Time.deltaTime * heightChangeSpeed);
            }
        }

        return new Vector3(targetX, currentHeight, transform.position.z);
    }

    /// <summary>
    /// Проверяет, находится ли игрок за пределами видимости камеры
    /// </summary>
    /// <returns>True если игрок вышел за границы камеры</returns>
    private bool IsPlayerOutOfCameraView()
    {
        if (target == null || cam == null) return false;

        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);
        // Проверяем выход за верхнюю или нижнюю границу
        return viewportPos.y < 0 || viewportPos.y > 1;
    }

    //<summary>
    // Определяет подходящий уровень высоты для камеры
    //</summary>
    //<returns>Значение высоты из массива heightLevels</returns>
    float GetHeightLevel()
    {
        foreach (var level in heightLevels)
        {
            if (target.position.y <= level)
            {
                return level;
            }
        }
        return heightLevels[heightLevels.Length - 1];
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
    // Запуск каст-сцены
    //</summary>
    public void PlayCutscene(Vector3 position, float zoom, float smoothness = 0.3f)
    {
        SetCameraLock(position);
        SetZoom(zoom);
        SetSmoothness(smoothness);
    }

    //<summary>
    // Выход из каст-сцены
    //</summary>
    public void EndCutscene()
    {
        ReleaseCameraLock();
    }
}
