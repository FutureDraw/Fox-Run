using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Tracking")]
    public Transform target;                 // ������, �� ������� ������� ������
    public Vector2 horizontalOffset = new Vector2(2f, 0.3f);  // �������� ������ �� X (���, ����)
    public float smoothTime = 0.3f;          // ����� ����������� �������� ������

    [Header("Vertical Tracking")]
    [Range(0f, 1f)]
    public float viewportTargetY = 0.45f;    // �������� ������� ������ �� ��������� � Viewport
    private float lowerTertie = 1f / 3f;
    private float upperTertie = 2f / 3f;
    public float baseVerticalSpeed = 10f;    // ������� �������� �������� �� ���������
    public float maxVerticalSpeed = 1000f;   // ������������ �������� �������� �� ���������

    [Header("Zoom")]
    public float zoomSpeed = 3f;             // �������� ��������� ����
    public float minZoom = 4f;               // ����������� ���
    public float maxZoom = 10f;              // ������������ ���
    public float targetZoom;                 // ������� ��� ������

    [Header("Camera Lock")]
    public bool isLocked;                    // ���� ���������� ������
    public Vector3 lockedPosition;           // ������� ��� ���������� ������

    private Camera cam;                      // ��������� ������
    private Vector3 velocity = Vector3.zero; // ������ �������� ��� SmoothDamp �� �����������
    private float verticalVelocity = 0f;     // �������� ��� ����������� �� ���������

    void Start()
    {
        // ������������� �����������
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoomInput();
    }

    void LateUpdate()
    {
        // ����� ��������� ������ � ����������� �� ��������� ����������
        if (isLocked)
            LockCameraBehavior();
        else
            FollowPlayerBehavior();
    }

    //<summary>
    // ������������ ���� ��� ��������� ���� ������
    //</summary>
    void HandleZoomInput()
    {
        if (Input.GetKey(KeyCode.Equals)) targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Minus)) targetZoom += zoomSpeed * Time.deltaTime;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 5f);
    }

    //<summary>
    // ��������� ������ ��� ���������� �� �������
    //</summary>
    void FollowPlayerBehavior()
    {
        if (target == null) return;

        // �������������� ��������
        float xOffset = Mathf.Lerp(
            horizontalOffset.x,
            horizontalOffset.y,
            Mathf.Abs(target.localScale.x)
        );
        float targetX = target.position.x + xOffset;

        // ������������ ���������
        float viewportY = cam.WorldToViewportPoint(target.position).y;
        float targetY = transform.position.y;

        if (viewportY < lowerTertie || viewportY > upperTertie)
        {
            // ����� ����� �� ������� ������ - ������� ������
            float idealCamY = target.position.y - (viewportTargetY - 0.5f) * (2f * cam.orthographicSize);

            // ��������� ��������������� ����������
            float deviation = viewportY > upperTertie
                ? (viewportY - upperTertie) / (1f - upperTertie)
                : (lowerTertie - viewportY) / lowerTertie;
            deviation = Mathf.Clamp01(deviation);

            // �������� ��������
            float followSpeed = Mathf.Lerp(baseVerticalSpeed, maxVerticalSpeed, deviation);

            // ������� ����������� �� ���������
            float newY = Mathf.SmoothDamp(
                transform.position.y,
                idealCamY,
                ref verticalVelocity,
                1f / followSpeed
            );
            targetY = newY;
        }
        // ����� ������ ������ - ��������� ������� ������

        Vector3 desiredPos = new Vector3(targetX, targetY, transform.position.z);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            smoothTime
        );
    }

    //<summary>
    // ��������� ������ � ��������������� ���������
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

    // API ��� �������� ��������

    //<summary>
    // ��������� ������ � ��������� �������
    //</summary>
    //<param name="position">������� ��� ���������� ������</param>
    public void SetCameraLock(Vector3 position)
    {
        isLocked = true;
        lockedPosition = position;
    }

    //<summary>
    // ������������ ������
    //</summary>
    public void ReleaseCameraLock()
    {
        isLocked = false;
    }

    //<summary>
    // ������������� ������� ��� ������
    //</summary>
    //<param name="zoom">�������� ���� (����� ���������� minZoom/maxZoom)</param>
    public void SetZoom(float zoom)
    {
        targetZoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    //<summary>
    // ������������� ��������� �������� ������
    //</summary>
    //<param name="value">����� �������� smoothTime</param>
    public void SetSmoothness(float value) => smoothTime = value;

    //<summary>
    // ������ ��������
    //</summary>
    public void PlayCutscene(Vector3 position, float zoom, float smoothness = 0.3f)
    {
        SetCameraLock(position);
        SetZoom(zoom);
        SetSmoothness(smoothness);
    }

    //<summary>
    // ����� �� ��������
    //</summary>
    public void EndCutscene()
    {
        ReleaseCameraLock();
    }
}
