using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Dialogue")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines; // Массив строк диалога
}
