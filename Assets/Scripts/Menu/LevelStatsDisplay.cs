using UnityEngine;
using TMPro;
using System.IO;

public class LevelStatsDisplay : MonoBehaviour
{
    [System.Serializable]
    public class LevelUI
    {
        public GameObject panel; // ��� ������ ������
        public TextMeshProUGUI passedText;
        public TextMeshProUGUI bestTimeText;
        public TextMeshProUGUI trophiesText;
    }

    public LevelUI[] levelUis; // ������ ������� ��� ������ �������

    public void LoadLevelStats(int levelIndex)
    {
        Debug.Log("�������� ���������� ��� ������: " + levelIndex);

        // ���������� ������ ������ �������� ������
        for (int i = 0; i < levelUis.Length; i++)
        {
            levelUis[i].panel.SetActive(i == levelIndex - 1);
        }

        if (levelIndex < 1 || levelIndex > levelUis.Length)
        {
            Debug.LogWarning("������������ ������ ������: " + levelIndex);
            return;
        }

        var ui = levelUis[levelIndex - 1];

        string path = Path.Combine(Application.persistentDataPath, $"level_{levelIndex}.json");

        // ���� ���� �� ����������, ������� ��������� ������
        if (!File.Exists(path))
        {
            Debug.Log("���� �� ������, ��������� ��������� ��������");
            ui.passedText.text = "���";
            ui.bestTimeText.text = "�";
            ui.trophiesText.text = "0";

            // ���� ��� �� ����������� ������ (�������)
            ui.passedText.color = Color.red;
            ui.bestTimeText.color = Color.red;
            ui.trophiesText.color = Color.red;
            return;
        }

        // ������ � �������������� ������
        string json = File.ReadAllText(path);
        PlayerRecord record = JsonUtility.FromJson<PlayerRecord>(json);

        // ���������, ������� �� �������
        if (!string.IsNullOrEmpty(record.time) && record.trophies > 0)
        {
            ui.passedText.text = "��";
            ui.passedText.color = Color.green;

            // ���� ����� ������, ��������� ���
            if (float.Parse(record.time.Replace(":", "").Replace(".", "")) < float.Parse(ui.bestTimeText.text.Replace(":", "").Replace(".", "")))
            {
                ui.bestTimeText.text = record.time;
            }

            // ���� ������� ������, ��������� ��������
            if (record.trophies > int.Parse(ui.trophiesText.text))
            {
                ui.trophiesText.text = $"{record.trophies}";
            }

            ui.bestTimeText.color = Color.green;
            ui.trophiesText.color = Color.green;
        }
        else
        {
            // ���� �� ��������, ������ �� �������
            ui.passedText.text = "���";
            ui.bestTimeText.text = "�";
            ui.trophiesText.text = "0";

            ui.passedText.color = Color.red;
            ui.bestTimeText.color = Color.red;
            ui.trophiesText.color = Color.red;
        }
    }
}
