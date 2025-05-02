using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;   // Панель диалога
    public TMP_Text dialogueText;      // Текст диалога
    public Image characterIcon;        // Иконка персонажа

    public float typingSpeed = 0.02f;  // Скорость печати

    private Queue<DialogueLine> lines = new Queue<DialogueLine>(); // Очередь для строк диалога
    private bool isTyping = false;     // Флаг, чтобы не начать новый диалог, пока старый не завершён

    private bool dialogueActive = false; // Флаг активности диалога

    public bool IsDialogueActive => dialogueActive; // Чтение активности

    // Метод для начала диалога
    public void StartDialogue(DialogueData dialogue)
    {
        dialoguePanel.SetActive(true);  // Показываем панель
        lines.Clear();                  // Очищаем очередь
        dialogueActive = true;          // Диалог активен

        // Добавляем все строки в очередь
        foreach (var line in dialogue.lines)
            lines.Enqueue(line);

        DisplayNextLine(); // Показываем первую строку
    }

    // Показать следующую строку
    public void DisplayNextLine()
    {
        if (isTyping) return;  // Если в процессе печати, не продолжаем

        if (lines.Count == 0)  // Если строки закончились
        {
            EndDialogue();  // Завершаем диалог
            return;
        }

        DialogueLine line = lines.Dequeue(); // Получаем следующую строку
        characterIcon.sprite = line.characterIcon; // Меняем иконку персонажа

        StopAllCoroutines(); // Останавливаем все предыдущие корутины
        StartCoroutine(TypeSentence(line.text)); // Запускаем корутину для печати текста
    }

    // Корутина для печати текста
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";  // Очищаем текст

        foreach (char letter in sentence) // Печатаем каждый символ
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // Задержка между символами
        }

        isTyping = false;  // Завершаем печать
    }

    // Завершение диалога
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);  // Скрываем панель
        dialogueActive = false;          // Диалог завершён
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))  // При нажатии пробела
        {
            DisplayNextLine();  // Показать следующую строку
        }
    }
}
