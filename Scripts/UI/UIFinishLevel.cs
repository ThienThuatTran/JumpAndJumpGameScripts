using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UIFinishLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI levelCompletedText;
    [SerializeField] private LocalizedString levelCompletedLocalizedString;

    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button creditsButton;

    [SerializeField] private GameObject starParent;
    [SerializeField] private Sprite starReward;
    [SerializeField] private float starDelayDuration = 0.3f;

    private int starCount;

    private float levelCompletedPercent;

    
    private void Start()
    {
        GameManager.Instance.OnLevelFinished += GameManager_OnFinishLevel;

        gameObject.SetActive(false);
        gameObject.SetActive(true);


        //FinishLevel();
    }

    private void GameManager_OnFinishLevel(object sender, System.EventArgs e)
    {
        
        CheckFinalLevel();

        ShowFinishedScore();


        PlayerPrefs.SetInt("hasGameProgress", 1);
        PlayerPrefs.SetInt(GameManager.Instance.GetCurrentLevelName() + "isCompleted", 1);

    }

    private void CheckFinalLevel()
    {
        if (GameManager.Instance.isFinalLevel)
        {
            nextLevelButton.gameObject.SetActive(false);
            creditsButton.gameObject.SetActive(true);
            creditsButton.Select();
        }
        else
        {
            nextLevelButton.Select();
        }
    }
    private void ShowFinishedScore()
    {
        gemsText.text = GameManager.Instance.GetCollectedGems().ToString();
        enemiesText.text = GameManager.Instance.GetDestroyedEnemies().ToString();
        playerHPText.text = PlayerStatus.Instance.GetPlayerHealth().GetHealth().ToString() + "/" + PlayerStatus.Instance.GetPlayerHealth().GetMaxHealth().ToString();
        levelCompletedPercent = GameManager.Instance.GetLevelCompletedPercent();

        StarRating(levelCompletedPercent);
        levelCompletedText.text = levelCompletedLocalizedString.GetLocalizedString(Mathf.RoundToInt(levelCompletedPercent * 100).ToString());
    }

    private void StarRating(float levelCompletedPc)
    {
        
        int star = 0;

        if (levelCompletedPc <= 0.3f)
        {
            star = 1;
        } 
        else if (levelCompletedPc <= 0.6f)
        {
            star = 2;
        } 
        else if (levelCompletedPc <= 0.8f)
        {
            star = 3;
        }
        else if(levelCompletedPc <= 0.95f)
        {
            star = 4;
        }else
        {
            star = 5;
        }

        int bestStar = PlayerPrefs.GetInt(GameManager.Instance.GetCurrentLevelName() + "StarReward" ,0);
        if (star > bestStar)
            PlayerPrefs.SetInt(GameManager.Instance.GetCurrentLevelName() + "StarReward", star);

        StartCoroutine(PopupCoroutine(star));
        
    }
    private IEnumerator PopupCoroutine(int star)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < star; i++)
        {
            
            starParent.transform.GetChild(i).GetComponent<Image>().sprite = starReward;
            SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.starPopupSFX, transform, 1f);
            yield return new WaitForSeconds(starDelayDuration);
        }
        if (star >=4) SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.starRateCompleteSFX, transform, 1f);
    }

    public void NextLevel()
    {
        LoadSceneManager.Instance.LoadLevel(GameManager.Instance.GetNextLevelName());

    }

    public void GoToMainMenu()
    {
        LoadSceneManager.Instance.LoadLevel(SceneToLoad.StartMenu.ToString());
    }

    public void GameCredits()
    {
        LoadSceneManager.Instance.LoadLevel(SceneToLoad.CreditScene.ToString());
    }

}
