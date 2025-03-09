using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIAllMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject gamePausedMenu;
    [SerializeField] private GameObject[] uIObjects;
    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        playerInputAction = InputManager.Instance.GetPlayerInputAction();


    }
    private void Start()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        if (GameManager.Instance == null)
        {
            return;
        }
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

    }

    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        SwitchUI();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        SwitchUI(gamePausedMenu);
    }


    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    public void SwitchUI(GameObject uIToOpen = null)
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        foreach (GameObject uIToClose in uIObjects)
        {
            uIToClose.SetActive(false);
        }
        if (uIToOpen != null)
        {
            uIToOpen.SetActive(true);
        }
    }


    public void NewGame()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        LoadSceneManager.Instance.LoadLevel(SceneToLoad.Level1.ToString());
        DeletePreviousSession();
        //PlayerPrefs.SetString("levelToLoad", SceneToLoad.Level1.ToString());
    }

    public void LoadStartMenu()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        SceneManager.LoadScene(SceneToLoad.StartMenu.ToString());
    }


    public void ExitGame()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    private int LevelCount()
    {
        int levelSceneCount = 0;
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneUtility.GetScenePathByBuildIndex(i).Contains("Level") && !SceneUtility.GetScenePathByBuildIndex(i).Contains(".5"))
            {
               
                levelSceneCount++;
            }
        }
       return levelSceneCount;
    }

    private void DeletePreviousSession()
    {
        int levelCount = LevelCount();

        for (int i = 1; i <= levelCount; i++)
        {
            PlayerPrefs.DeleteKey("Level" + i + "Tutorial" + "isCompleted");

            PlayerPrefs.DeleteKey("Level" + i + "isCompleted");
            PlayerPrefs.DeleteKey("Level" + i + "StarReward");
            PlayerPrefs.DeleteKey("Level" + i + "isUnlocked");
        }

        PlayerPrefs.DeleteKey("hasGameProgress");
    }

    
}

public enum SceneToLoad
{
    StartMenu,
    LoadingScene,
    Level1,
    CreditScene

}

