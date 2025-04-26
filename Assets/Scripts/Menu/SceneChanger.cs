using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string gameSceneName;
    public Animator animator; // ���� ������� ���� Animator � ��������� ���������
    public string fadeAnimationName = "Fade"; // �������� �������� ���������
    public float delayBeforeSceneLoad = 0.5f; // ������� ����� �� ����� ����� (����� ��������)

    void Start()
    {
        // ��������� ��������
        animator.Play(fadeAnimationName);

        // ��������� �������� ��� �������� � ����� �����
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        // ��� ������ �����
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // ������ �����
        SceneManager.LoadScene(gameSceneName);
    }
}
