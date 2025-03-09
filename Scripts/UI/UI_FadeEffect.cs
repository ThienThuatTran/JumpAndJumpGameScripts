using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour
{
    [SerializeField] private GameObject allOtherUI;
    private Image fadeImage;
    public static UI_FadeEffect Instance;
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
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

    }
    private void Start()
    {
        
    }
    public void ScreenFade(float targetAlpha, float duration, bool disableAllOtherUI, Action onComplete = null)
    {
        if (fadeImage == null) return;
        StartCoroutine(FadeCoroutine(targetAlpha, duration, onComplete));
        if (disableAllOtherUI)
        {
            allOtherUI.SetActive(false);
        }
    }
    private IEnumerator FadeCoroutine(float targetAlpha, float duration, Action onComplete)
    {
        float time = 0;
        Color currentColor = fadeImage.color;
        float startAlpha = currentColor.a;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            yield return null;
        }
        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        onComplete?.Invoke();

    }

}
