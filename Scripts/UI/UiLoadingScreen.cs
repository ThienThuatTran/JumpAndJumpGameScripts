using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider loadingSceneSlider;
    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            LoadLevel();
            isFirstUpdate = false;
        }
    }

    private IEnumerator LoadLevelAsyncCoroutine(string levelScene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelScene);

        while (!asyncOperation.isDone)
        {
            //Debug.Log(asyncOperation.progress);
            loadingSceneSlider.value = asyncOperation.progress;
            yield return null;
        }
    }

    private void LoadLevel()
    {
        string levelToLoad = PlayerPrefs.GetString("levelToLoad", "Level2");
        StartCoroutine(LoadLevelAsyncCoroutine(levelToLoad));
    }
}
