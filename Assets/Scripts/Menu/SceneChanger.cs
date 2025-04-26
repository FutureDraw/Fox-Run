using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string gameSceneName;
    public Animator animator; // сюда привяжи твой Animator с анимацией затухания
    public string fadeAnimationName = "Fade"; // название анимации затухания
    public float delayBeforeSceneLoad = 0.5f; // сколько ждать до смены сцены (длина анимации)

    void Start()
    {
        // Запускаем анимацию
        animator.Play(fadeAnimationName);

        // Запускаем корутину для ожидания и смены сцены
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        // Ждём нужное время
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // Меняем сцену
        SceneManager.LoadScene(gameSceneName);
    }
}
