using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    public bool affectPosition = true;
    public Vector3 cameraPosition;
    public bool useTriggerPosition;

    public bool affectZoom;
    [Range(1f, 20f)] public float cameraZoom = 5f;

    [Header("Transition Settings")]
    public float positionSmoothness = 0.3f;
    public float zoomSmoothness = 5f;

    private CameraController _cameraController;
    private float _previousZoom;
    private Vector3 _previousPosition;
    private bool _isActive;

    void Start()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
        if (useTriggerPosition) cameraPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (affectZoom)
        {
            _previousZoom = _cameraController.targetZoom;
            _cameraController.SetZoom(cameraZoom);
        }

        if (affectPosition)
        {
            _cameraController.smoothTime = positionSmoothness;
            _cameraController.SetCameraLock(cameraPosition);
        }

        _isActive = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !_isActive) return;

        if (affectZoom)
        {
            _cameraController.SetZoom(_previousZoom);
        }

        if (affectPosition)
        {
            _cameraController.smoothTime = positionSmoothness;
            _cameraController.ReleaseCameraLock();
        }

        _isActive = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!affectPosition) return;

        Vector3 pos = useTriggerPosition ? transform.position : cameraPosition;

        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic) return;

        float size = affectZoom ? cameraZoom : cam.orthographicSize;
        float aspect = cam.aspect;

        float height = size * 2f;
        float width = height * aspect;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(pos, new Vector3(width, height, 0f));
    }

}