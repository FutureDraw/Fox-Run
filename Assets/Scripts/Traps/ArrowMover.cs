using UnityEngine;

/// <Summary>
/// ��������� ��������� ������ �� ������,
/// ���������� �� �������� ��� ��� ������������.
/// </Summary>
public class ArrowMover : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private float _lifetime;

    /// <Summary>
    /// ������������� ���������� ����� ������
    /// </Summary>
    public void Initialize(Vector2 direction, float speed, float lifetime)
    {
        _direction = direction.normalized;
        _speed = speed;
        _lifetime = lifetime;
        // ���������� ������ ����� _lifetime ������
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        // ������� ������ �����
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��� ����� ������������ ���������� ������
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0f);
    }

}
