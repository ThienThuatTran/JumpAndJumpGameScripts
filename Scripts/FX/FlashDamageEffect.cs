using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDamageEffect : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    private float flashTime = 0.25f;
    [SerializeField] private AnimationCurve flashSpeedCurve;
    [SerializeField] private float flashTimeOffset = 0.2f;
    private SpriteRenderer playerRenderer;
    private Material material;
    private Coroutine damageFlashCoroutiner;
    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        flashTime = PlayerStatus.Instance.GetKnockTime() +flashTimeOffset;
        material = playerRenderer.material;
    }

    private IEnumerator DamageFalsher()
    {
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elaspedTime = 0f;
        while (elaspedTime < flashTime)
        {
            elaspedTime += Time.deltaTime;
            //float conpensationTime = elaspedTime
            currentFlashAmount = Mathf.Lerp(0f, flashSpeedCurve.Evaluate(elaspedTime/flashTime), flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
        //yield return null;
        SetFlashAmount(0);
    }
    public void CallDamageFlash()
    {
        damageFlashCoroutiner = StartCoroutine(DamageFalsher());
    }
    private void SetFlashColor()
    {
        material.SetColor("_FlashColor", flashColor);
    }

    private void SetFlashAmount(float flashAmount)
    {
        material.SetFloat("_FlashAmount", flashAmount);
    }

    public void CancelDamageFX()
    {
        StopAllCoroutines();
        SetFlashAmount(0);
    }
}
