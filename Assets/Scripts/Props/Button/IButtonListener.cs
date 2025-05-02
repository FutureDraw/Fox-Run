using UnityEngine;

// <summary>
// Интерфейс для объектов, реагирующих на переключение кнопки
// </summary>
public interface IButtonListener
{
    void Activate();
    void Deactivate();
}
