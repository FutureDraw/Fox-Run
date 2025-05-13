using UnityEditor;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс для смены сцен
/// </summary>
public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public Animator animator;
    public AnimationClip fadeAnimationClip;
    public float delayBeforeSceneLoad = 0.5f;

    public PlayerTimer timer;

    void Start()
    {
        animator.Play(fadeAnimationClip.name);

        StartCoroutine(WaitAndLoadScene());
        PlayerTimer.Instance.ElapsedTime = 0f;
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad); // Время ошидания для смены сценны


        SceneManager.LoadScene(sceneName);
    }
}
