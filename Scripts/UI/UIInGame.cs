using TMPro;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitsText;

    private void UIInGame_OnFruitsChange(object sender, System.EventArgs e)
    {
        DisplayFruits();
    }

    private void Start()
    {
        GameManager.Instance.OnGemsChange += UIInGame_OnFruitsChange;
        DisplayFruits();
    }

    private void DisplayFruits()
    {
        fruitsText.text = GameManager.Instance.GetCollectedGems().ToString();
    }
}
