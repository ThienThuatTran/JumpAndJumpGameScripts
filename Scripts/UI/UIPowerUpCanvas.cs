using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UIPowerUpCanvas : MonoBehaviour
{
    [SerializeField] private LocalizedString powerUpTutorialLocalizedString;
    [SerializeField] private TextMeshProUGUI powerUpTutorialText;
    [SerializeField] private string actionKeyString;
    [SerializeField] private Button okButton;
    private void OnEnable()
    {
        okButton.Select();
        InputManager.Instance.SetUIMapsInputAction();
    }
    public void TurnOffPowerUpCanvas()
    {
        
        gameObject.SetActive(false);
        
    }

    private string TutorialButtonText()
    {
        InputAction inputAction = InputManager.Instance.GetPlayerInputAction().FindAction(actionKeyString);
        string buttonText = InputControlPath.ToHumanReadableString(
           inputAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        return buttonText;
    }

    public void UpdatePowerUpTutorial()
    {
        powerUpTutorialText.text = powerUpTutorialLocalizedString.GetLocalizedString(TutorialButtonText());
    }

    private void OnDisable()
    {
        InputManager.Instance.SetPlayerMapsInputAction();
    }
}
