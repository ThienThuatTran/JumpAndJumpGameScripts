using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadLevel(string levelSceneName)
    {
        UI_FadeEffect.Instance.ScreenFade(1, 1, true ,LoadLoadingScene);
        PlayerPrefs.SetString("levelToLoad", levelSceneName);
    }

    private void LoadLoadingScene()
    {
        SceneManager.LoadScene(SceneToLoad.LoadingScene.ToString());
        
    }
}
