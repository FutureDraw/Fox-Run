using UnityEngine;

[System.Serializable]
/// <summary>
/// Диалог
/// </summary>
public class DialogueLine
{
    public string characterName;    // Имя персонажа
    public Sprite characterIcon;    // Иконка персонажа
    [TextArea(2, 5)] public string text;  // Текст диалога
}
