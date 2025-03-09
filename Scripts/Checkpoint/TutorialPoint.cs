using UnityEngine;

public class TutorialPoint : MonoBehaviour
{
    [SerializeField] private UI_FadeText fadeTextFX;
    
    private bool isFirstPopUp = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("wall jump tutorial");
        if (!isFirstPopUp)
        {
            return;
        }

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("wall jump tutorial");
            fadeTextFX.TextFade(0, 4, DisableText);
            isFirstPopUp = false;
        }
        

    }

    private void DisableText()
    {
        fadeTextFX.gameObject.SetActive(false);
    }
}
