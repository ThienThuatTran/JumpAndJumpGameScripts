using TMPro;
using UnityEngine;

public class UILevelTitleText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTitleText;
    private UI_FadeText fadeTextFX;

    private void Awake()
    {
        fadeTextFX = GetComponent<UI_FadeText>();
    }
    private void Start()
    {
        if (!GameManager.Instance.isTutorialLevel)
        {
            levelTitleText.text = "LEVEL " + GameManager.Instance.GetCurrentLevelIndex();
            Invoke(nameof(FadeLevelTitleText), 1);
        }
        else
        {
            levelTitleText.text = "";
        }
        
        
    }

    private void FadeLevelTitleText()
    {
        fadeTextFX.TextFade(0, 1.5f);
    }
}
