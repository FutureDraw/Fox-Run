using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;    // Имя персонажа
    public Sprite characterIcon;    // Иконка персонажа
    [TextArea(2, 5)] public string text;  // Текст диалога
}
