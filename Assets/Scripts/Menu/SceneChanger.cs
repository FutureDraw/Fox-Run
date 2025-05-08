#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    public Animator animator;
    public AnimationClip fadeAnimationClip;
    public float delayBeforeSceneLoad = 0.5f;

    void Start()
    {
        animator.Play(fadeAnimationClip.name);

        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad); // Время ошидания для смены сценны

#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            string sceneName = sceneAsset.name;
            SceneManager.LoadScene(sceneName);
        }
#endif
    }
}
