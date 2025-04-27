using UnityEngine;

public class GrabberController : MonoBehaviour, ITrap
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;        // �������� �������� grabber'� � ����
    [SerializeField] private int _stopTime = 5;        // ����� ��������� ������
    [SerializeField] private float _reachThreshold = 0.1f;  // ����� ���������� ����

    [Header("References")]
    [SerializeField] private Transform _targetPoint;   // ������� ����� �����������

    private Transform _playerTransform;                // ������ �� ��������� ������
    private bool _isPulling;                           // ���� ��������� �����������
    private bool _isGrabbed;                           // ���� ������� ������
    private PlayerController _playerController;        // ������ �� ���������� ������
    private Vector3 _finalPosition;                    // �������� ������� grabber'�

    private void Start()
    {
        // ��������� ���������� ��� ��������
        GetComponent<CircleCollider2D>().isTrigger = true;

        // ��������� ������ �� ������
        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController != null)
        {
            _playerTransform = _playerController.transform;
        }

        Debug.Log("Grabber initialized and armed");
    }

    private void Update()
    {
        // ���� grabber �������� � ����
        if (_isPulling)
        {
            // ������� grabber � ���� � �������� ���������
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPoint.position,
                _speed * Time.deltaTime
            );

            // ��������� ���������� ����
            if (Vector3.Distance(transform.position, _targetPoint.position) < _reachThreshold)
            {
                _isPulling = false;
                _finalPosition = transform.position;  // ���������� �������� �������
            }
        }

        // ���� ����� ��������
        if (_isGrabbed)
        {
            // ����������� ������ � ������� ������� grabber'� (���� ��������) ��� � �������� �������
            _playerTransform.position = _isPulling ? transform.position : _finalPosition;
        }
    }

    //<Summary>
    // ������ ��������� ������
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
    // ��������� ������������ � �������
    //</Summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isGrabbed)
        {
            Debug.Log("Player grabbed");
            _isGrabbed = true;    // ���������� ������
            _isPulling = true;    // �������� ��������
            StopPlayer(_stopTime); // ������������� ������
            Destroy(gameObject, _stopTime); // ���������� grabber ����� �������� �����
        }
    }

    //<Summary>
    // �������������
    //</Summary>
    public void SlowPlayer(float time, float strength) { }

    //<Summary>
    // �������������
    //</Summary>
    public void KillPlayer() { }
}