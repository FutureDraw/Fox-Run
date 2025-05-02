using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Tracking")]
    public Transform target;                // ������, �� ������� ������� ������
    public Vector2 horizontalOffset = new Vector2(2f, 0.3f);  // �������� ������ �� X (���, ����)
    public float smoothTime = 0.3f;          // ����� ����������� �������� ������

    [Header("Height Levels")]
    public float[] heightLevels;             // ������ ����� ��� ������
    public float heightChangeSpeed = 2f;     // �������� ��������� ������ ������

    [Header("Zoom")]
    public float zoomSpeed = 3f;             // �������� ��������� ����
    public float minZoom = 4f;               // ����������� ���
    public float maxZoom = 10f;              // ������������ ���

    [Header("Camera Lock")]
    public bool isLocked;                    // ���� ���������� ������
    public Vector3 lockedPosition;           // ������� ��� ���������� ������

    private Camera cam;                      // ��������� ������
    private Vector3 velocity = Vector3.zero; // ������ �������� ��� SmoothDamp
    private float currentHeight;             // ������� ������ ������
    public float targetZoom;                 // ������� ��� ������
    private bool isHeightTransition;         // ���� �������� ����� �������� �����
    private bool isFollowingDueToExit;       // ���� ��� ������������ ������ ������ �� ������� ������

    void Start()
    {
        // ������������� �����������
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
        // ����� ��������� ������ � ����������� �� ��������� ����������
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
    // ��������� ������� ������� ������ �� ������ ������� ������
    //</summary>
    //<param name="player">���������� ������ ��� �������� ���������</param>
    //<returns>������ ������� ������� ������</returns>
    Vector3 CalculateCameraPosition(PlayerController player)
    {
        // ��������� �������� �� X � ����������� �� �������� ����
        float xOffset = Mathf.Lerp(
            horizontalOffset.x,
            horizontalOffset.y,
            Mathf.Abs(target.localScale.x)
        );

        float targetX = target.position.x + xOffset;
        float targetY = currentHeight;

        // ���� ����� �� ����� - ������ ������ ������
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

            // ���������, ����� �� ����� �� ������� ������
            if (IsPlayerOutOfCameraView())
            {
                isFollowingDueToExit = true; // ���������� ����� ����������
            }

            // ���� ����� ����� �� ������� - ������� �� ��� �� ���������
            if (isFollowingDueToExit)
            {
                targetY = target.position.y;
                currentHeight = Mathf.Lerp(currentHeight, targetY, Time.deltaTime * heightChangeSpeed);
            }
        }

        return new Vector3(targetX, currentHeight, transform.position.z);
    }

    /// <summary>
    /// ���������, ��������� �� ����� �� ��������� ��������� ������
    /// </summary>
    /// <returns>True ���� ����� ����� �� ������� ������</returns>
    private bool IsPlayerOutOfCameraView()
    {
        if (target == null || cam == null) return false;

        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);
        // ��������� ����� �� ������� ��� ������ �������
        return viewportPos.y < 0 || viewportPos.y > 1;
    }

    //<summary>
    // ���������� ���������� ������� ������ ��� ������
    //</summary>
    //<returns>�������� ������ �� ������� heightLevels</returns>
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
    // ������ ����-�����
    //</summary>
    public void PlayCutscene(Vector3 position, float zoom, float smoothness = 0.3f)
    {
        SetCameraLock(position);
        SetZoom(zoom);
        SetSmoothness(smoothness);
    }

    //<summary>
    // ����� �� ����-�����
    //</summary>
    public void EndCutscene()
    {
        ReleaseCameraLock();
    }
}
