using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private string[] languageTextArray;
    public static LocalizationManager Instance;

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
    public void LoadLanguage()
    {
        int languageID = PlayerPrefs.GetInt("languageIndex",0);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
    }

    public string[] GetLanguageTextArray()
    {
        return languageTextArray;
    }
    
}


