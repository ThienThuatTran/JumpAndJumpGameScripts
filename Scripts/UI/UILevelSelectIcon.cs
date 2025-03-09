using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelSelectIcon : MonoBehaviour
{
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Transform starRewardParent;
    [SerializeField] private Sprite yellowStar;
    [SerializeField] private string levelName;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Button levelSelectButton;

    private const int maxStar = 5;
    private void Start()
    {
        levelNameText.text = levelName;
        if (IsLevelCompleted())
        {
            levelSelectButton.interactable = true;
            SetLockIcon(false);
            starRewardParent.gameObject.SetActive(true);
            UpdateStarReward();
        }
        else
        {
            levelSelectButton.interactable = false;
            SetLockIcon(true);
            starRewardParent.gameObject.SetActive(false);
        }
    }

    private bool IsLevelCompleted()
    {
        return PlayerPrefs.GetInt(levelName + "isUnlocked", 0) ==1;
    }
    public void LoadLevelSelected()
    {
        //SceneManager.LoadScene(levelName);
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        LoadSceneManager.Instance.LoadLevel(levelName);
    }

    private void SetLockIcon(bool enable)
    {
        lockIcon.SetActive(enable);
    }

    private void UpdateStarReward()
    {
        int starCount = PlayerPrefs.GetInt(levelName + "StarReward", 0);
        Debug.Log(starCount);
        for (int i = 0; i < starCount; i++)
        {
            starRewardParent.GetChild(i).GetComponent<Image>().sprite = yellowStar;
        }
    }
}
