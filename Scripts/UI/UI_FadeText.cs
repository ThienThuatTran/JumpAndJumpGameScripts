using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public class UI_FadeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fadeText;
    [SerializeField] private string actionKeyString;

    private void Start()
    {
        
    }
    private IEnumerator FadeCoroutine(float targetAlpha, float duration, Action onComplete)
    {
        float time = 0;
        Color currentColor = fadeText.color;
        float startAlpha = currentColor.a;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            yield return null;
        }
        fadeText.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        onComplete?.Invoke();

    }

    public void TextFade(float targetAlpha, float duration, Action onComplete = null)
    {

        fadeText.gameObject.SetActive(true);
        if (actionKeyString != "") fadeText.text += TutorialButtonText();
        if (fadeText == null) return;
        StartCoroutine(FadeCoroutine(targetAlpha, duration, onComplete));
    }

    private string TutorialButtonText()
    {
        InputAction inputAction = InputManager.Instance.GetPlayerInputAction().FindAction(actionKeyString);
        string buttonText = InputControlPath.ToHumanReadableString(
           inputAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        return buttonText;
    }

}
