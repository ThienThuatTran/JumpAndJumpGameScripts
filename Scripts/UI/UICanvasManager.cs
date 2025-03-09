using System;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasManager : MonoBehaviour
{
    public static UICanvasManager Instance;

    [SerializeField] private GameObject inGameCanvas;
    [SerializeField] private GameObject finishLevelGameObject;
    [SerializeField] private GameObject gameOverGameObject;

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
        }

    }

    private void Start()
    {
        GameManager.Instance.OnLevelFinished += GameManager_OnLevelFinished;
        GameManager.Instance.OnGameOver += GameOver_OnGameOver;
    }

    private void GameOver_OnGameOver(object sender, EventArgs e)
    {
        OpenUIGameOver();

        InputManager.Instance.GetPlayerInputAction().Player.Disable();

    }

    private void GameManager_OnLevelFinished(object sender, EventArgs e)
    {
        OpenUILevelFinished();

        InputManager.Instance.SetUIMapsInputAction();
    }

    private void OpenUILevelFinished()
    {
        finishLevelGameObject.SetActive(true);
        inGameCanvas.SetActive(false);
    }

    private void OpenUIGameOver()
    {
        gameOverGameObject.SetActive(true);
        inGameCanvas.SetActive(false);
    }
}
