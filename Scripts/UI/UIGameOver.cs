using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private Button replayButton;

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        replayButton.Select();
    }

    public void RestartLevel()
    {
        GameManager.Instance.LoadCurrentLevel();
    }

    public void GotoMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
