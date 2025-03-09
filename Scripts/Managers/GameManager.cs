using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    public event EventHandler OnGemsChange;

    public event EventHandler OnLevelFinished;
    public event EventHandler OnGameOver;
    public event EventHandler OnRespawnPlayer;

    private Transform respawnTransform;
    private Transform startTransform;

    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject items;

    


    [SerializeField] private int currentSceneIndex;
    private int nextSceneIndex;
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private bool hasPowerUp = false;
    
    public GameObject playerPrefab;

    
    
    public bool isTutorialLevel = false;
    public bool isFinalLevel = false;

    private int collectedGems;
    private int totalGems;
    private int destroyedEnemies;
    private int totalEnemies;

    private float levelCompletedPercent;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Start()
    {
        UI_FadeEffect.Instance.ScreenFade(0, 1f, false);

        startTransform = GameObject.FindWithTag("StartPoint").transform;
        Player.Instance.transform.position = startTransform.position;
        respawnTransform = startTransform;

        nextSceneIndex = currentSceneIndex + 1;

        totalEnemies = enemies.transform.childCount;
        totalGems = items.transform.childCount;
        destroyedEnemies = 0;
        collectedGems = 0;

        isCompleted = PlayerPrefs.GetInt(GetCurrentLevelName() + "isCompleted", 0) == 1;

        if (!isTutorialLevel) PlayerPrefs.SetInt(GetCurrentLevelName() + "isUnlocked", 1);


    }
    public void RespawnPlayer()
    {
        Player.Instance.transform.position =  respawnTransform.position;
        Player.Instance .transform.rotation = Quaternion.identity;

        Player.Instance.gameObject.SetActive(true);

        UI_FadeEffect.Instance.ScreenFade(0, 2, false);
        //player = playerGameObject.GetComponent<Player>();
        OnRespawnPlayer?.Invoke(this, EventArgs.Empty);  
    }

    public void AddCollectedGems()
    {
        collectedGems++;
        OnGemsChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetCollectedGems()
    {
        return collectedGems;
    }

    public void AddDestroyedEnemies()
    {
        destroyedEnemies++;
    }

    public int GetDestroyedEnemies()
    {
        return destroyedEnemies;
    }

    public void GameOver()
    {
        
        OnGameOver?.Invoke(this, EventArgs.Empty);
        //player.gameObject.SetActive(false);
    }

    public int GetTotalGems()
    {
        return totalGems;
    }

    public int GetTotalEnemies()
    {
        return totalEnemies;
    }

    public void NotifyLevelFinished()
    {
        OnLevelFinished?.Invoke(this, EventArgs.Empty);
    }

    public float GetLevelCompletedPercent()
    {
        float playerHPPercent = PlayerStatus.Instance.GetPlayerHealth().GetHealthPercent();
        levelCompletedPercent = (float)collectedGems / totalGems * 0.55f + (float)destroyedEnemies / totalEnemies * 0.35f + playerHPPercent * 0.1f;
        Debug.Log(playerHPPercent);
        return levelCompletedPercent;
    }

    public string GetCurrentLevelName()
    {
        return "Level" + currentSceneIndex;
    }

    public int GetCurrentLevelIndex()
    {
        return currentSceneIndex;
    }

    public string GetNextLevelName()
    {
        
        string nextSceneName = "Level" + nextSceneIndex;
        if (isTutorialLevel )
        {
            return nextSceneName;
        }

        if (!(PlayerPrefs.GetInt(GetCurrentLevelName() + "Tutorial" + "isCompleted", 0) ==1) && hasPowerUp)
        {
            return "Level" + currentSceneIndex.ToString() + ".5";
            
        }
        else
        {
            return nextSceneName;
        }
    }

    public void LoadCurrentLevel()
    {
        UI_FadeEffect.Instance.ScreenFade(1, 1, true, LoadCurrentLevelScene);
    }

    private void LoadCurrentLevelScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneToLoad.LoadingScene.ToString());
        PlayerPrefs.SetString("levelToLoad", GetCurrentLevelName());
    }

    public void LoadNextLevel()
    {
        UI_FadeEffect.Instance.ScreenFade(1, 1, true, LoadNextLevelScene);
        
    }

    private void LoadNextLevelScene()
    {
        //SceneManager.LoadScene(GetNextLevelName());
        SceneManager.LoadScene(SceneToLoad.LoadingScene.ToString());
        PlayerPrefs.SetString("levelToLoad", GetNextLevelName());
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        UI_FadeEffect.Instance.ScreenFade(1, 0.5f, true, LoadMainMenuScene);
    }

    private void LoadMainMenuScene()
    {
        
        SceneManager.LoadScene(SceneToLoad.StartMenu.ToString());
    }


    public void PauseGame()
    {
        //UICanvasManager.Instance.OpenPausedMenu();
        OnGamePaused?.Invoke(this, EventArgs.Empty);
        Time.timeScale = 0;
        InputManager.Instance.SetUIMapsInputAction();
    }

    public void ResumeGame()
    {
        //UICanvasManager.Instance.ClosePausedMenu();
        OnGameResumed?.Invoke(this, EventArgs.Empty);
        InputManager.Instance.SetPlayerMapsInputAction();
        Time.timeScale = 1;

    }

    public void UpdateRespawnponit(Transform newRespawnTransform)
    {
        respawnTransform = newRespawnTransform;
    }

}
