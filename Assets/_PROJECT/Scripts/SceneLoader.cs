using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private CanvasGroup transitionCanvasGroup;
    [SerializeField] private float transitionDuration = 0.5f;
    
    private static void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void SwitchWithTransition(int sceneIndex)
    {
        Time.timeScale = 1.0f;
        StartCoroutine(TransitionLerp(sceneIndex));
    }
    private IEnumerator TransitionLerp(int sceneIndex)
    {
        var timeElapsed = 0.0f;
        while (timeElapsed < transitionDuration)
        {
            transitionCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transitionCanvasGroup.alpha = 1f;
        SwitchScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}