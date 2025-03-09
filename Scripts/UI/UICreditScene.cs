using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICreditScene : MonoBehaviour
{
    private bool isFirstUpdate = true;
    private void Start()
    {
    }
    private void Update()
    {
        
        if (isFirstUpdate)
        {
            UI_FadeEffect.Instance.ScreenFade(0, 1, false);
            isFirstUpdate = false;
        }
        return;

    }


    private void LoadStartMenuScene()
    {
        SceneManager.LoadScene(SceneToLoad.StartMenu.ToString());
    }
}
