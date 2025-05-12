using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Dialogue")]
/// <summary>
/// Информация по диалогам
/// </summary>
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines; // Массив строк диалога
}
