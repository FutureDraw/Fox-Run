using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;   // ������ �������
    public TMP_Text dialogueText;      // ����� �������
    public Image characterIcon;        // ������ ���������

    public float typingSpeed = 0.02f;  // �������� ������

    private Queue<DialogueLine> lines = new Queue<DialogueLine>(); // ������� ��� ����� �������
    private bool isTyping = false;     // ����, ����� �� ������ ����� ������, ���� ������ �� ��������

    private bool dialogueActive = false; // ���� ���������� �������

    public bool IsDialogueActive => dialogueActive; // ������ ����������

    // ����� ��� ������ �������
    public void StartDialogue(DialogueData dialogue)
    {
        dialoguePanel.SetActive(true);  // ���������� ������
        lines.Clear();                  // ������� �������
        dialogueActive = true;          // ������ �������

        // ��������� ��� ������ � �������
        foreach (var line in dialogue.lines)
            lines.Enqueue(line);

        DisplayNextLine(); // ���������� ������ ������
    }

    // �������� ��������� ������
    public void DisplayNextLine()
    {
        if (isTyping) return;  // ���� � �������� ������, �� ����������

        if (lines.Count == 0)  // ���� ������ �����������
        {
            EndDialogue();  // ��������� ������
            return;
        }

        DialogueLine line = lines.Dequeue(); // �������� ��������� ������
        characterIcon.sprite = line.characterIcon; // ������ ������ ���������

        StopAllCoroutines(); // ������������� ��� ���������� ��������
        StartCoroutine(TypeSentence(line.text)); // ��������� �������� ��� ������ ������
    }

    // �������� ��� ������ ������
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";  // ������� �����

        foreach (char letter in sentence) // �������� ������ ������
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // �������� ����� ���������
        }

        isTyping = false;  // ��������� ������
    }

    // ���������� �������
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);  // �������� ������
        dialogueActive = false;          // ������ ��������
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))  // ��� ������� �������
        {
            DisplayNextLine();  // �������� ��������� ������
        }
    }
}
