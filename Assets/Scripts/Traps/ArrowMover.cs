using UnityEngine;

/// <Summary>
/// Управляет движением стрелы по прямой, уничтожает по таймауту или при столкновении.
/// </Summary>
public class ArrowMover : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private float _lifetime;

    /// <Summary>
    /// Инициализация параметров полёта стрелы
    /// </Summary>
    public void Initialize(Vector2 direction, float speed, float lifetime)
    {
        _direction = direction.normalized;
        _speed = speed;
        _lifetime = lifetime;
        // Уничтожаем стрелу через _lifetime секунд
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        // Двигаем стрелу вперёд
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // При любом столкновении уничтожаем стрелу
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0f);
    }

}
