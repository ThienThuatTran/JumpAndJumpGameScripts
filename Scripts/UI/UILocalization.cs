using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class UILocalization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI languageText;
    private int languageID;

    private void Start()
    {
        languageID = PlayerPrefs.GetInt("languageIndex", 0);
        Debug.Log(LocalizationSettings.AvailableLocales.Locales.Count);
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        UpdateLanguageText();
    }

    public void PreviousLanguageOption()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        languageID--;
        if (languageID < 0)
        {
            languageID = LocalizationSettings.AvailableLocales.Locales.Count - 1;
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
        
        //languageText.text = Language.English.ToString();
    }

    public void NextLanguageOption()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        languageID++;
        if (languageID > LocalizationSettings.AvailableLocales.Locales.Count-1)
        {
            languageID = 0;
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
       
    }

    private void SaveLanguage()
    {
        PlayerPrefs.SetInt("languageIndex", languageID);
    }


    private void LocalizationSettings_SelectedLocaleChanged(UnityEngine.Localization.Locale obj)
    {
        UpdateLanguageText();
        SaveLanguage();
    }

    private void UpdateLanguageText()
    {
        languageText.text = LocalizationManager.Instance.GetLanguageTextArray()[languageID];
    }
}
