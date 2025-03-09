using System.Collections;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private Animator anim;
    private bool active = false;
    [SerializeField] private bool canBeReactivate = false;
    [SerializeField] private float finishCheckPointRange = 15f;
    private bool hasEnemyInRange = false;
    private bool isFirstPopUp = true;
    private float finishLevelDelayDuration = 2f;
    [SerializeField] private UI_FadeText fadeTextFX;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        hasEnemyInRange = HasEnemyInRange();


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && !canBeReactivate)
        {
            return;
        }
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && !hasEnemyInRange)
        {

            if (!active && !canBeReactivate) { AllowReactivate(); }
            ActivateCheckpoint();
            StartCoroutine(FinishLevelDelayCoroutine());
        }
        else if (player != null && hasEnemyInRange)
        {
            if (isFirstPopUp)
            {
                isFirstPopUp = false;
                fadeTextFX.TextFade(0, 4, DisableText);
            }
        }
    }

    private void DisableText()
    {
        fadeTextFX.gameObject.SetActive(false);
    }

    private void ActivateCheckpoint()
    {
        active = true;
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.levelCompleteSFX, transform, 1f);
        anim.SetTrigger("activate");
        //GameManager.Instance.UpdateRespawnPosition(transform);
    }

    private void AllowReactivate()
    {
        canBeReactivate = true;
    }

    private IEnumerator FinishLevelDelayCoroutine()
    {
        yield return new WaitForSeconds(finishLevelDelayDuration);
        GameManager.Instance.NotifyLevelFinished();
    }

    private bool HasEnemyInRange()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, finishCheckPointRange);
        if (collider2Ds.Length == 0) { return false; }

        foreach (Collider2D collider2D in collider2Ds)
        {
            Enemy enemy = collider2D.gameObject.GetComponent<Enemy>();
            if (enemy != null) return true;
        }
        return false;
    }
}
