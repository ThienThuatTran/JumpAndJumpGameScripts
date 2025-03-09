using UnityEngine;
using UnityEngine.UI;

public class UIStartMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButtonGO;
    [SerializeField] private GameObject exitButtonGO;
    private bool isFirstUpdate = true;


    private void Start()
    {
        

#if UNITY_STANDALONE_WIN
        exitButtonGO.SetActive(true);
#endif
    }
    private void LateUpdate()
    {
        if (isFirstUpdate)
        {
            LocalizationManager.Instance.LoadLanguage();

            if (!(PlayerPrefs.GetInt("hasGameProgress", 0) == 1))
            {
                continueButtonGO.SetActive(false);
            }
            UI_FadeEffect.Instance.ScreenFade(0, 1f, false);
            isFirstUpdate = false;
        }
    }
}
