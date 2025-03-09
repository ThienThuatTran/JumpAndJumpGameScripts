using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    private enum PowerUp
    {
        Dash,
        WallJump,
        DoubleJump
    }
    [SerializeField] private GameObject powerUpCanvas;
    [SerializeField] private Transform collectedItemFX;
    [SerializeField] private float powerUpPopUpDelay = 1f;
    [SerializeField] private PowerUp powerUpName;
    private void Start()
    {
        powerUpCanvas.SetActive(true);
        powerUpCanvas.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {

            Instantiate(collectedItemFX, transform.position, Quaternion.identity);
            SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.powerUpSFX, transform, 1f);
            //Invoke(nameof(PowerUpPopUp), powerUpPopUpDelay);

            PowerUpPopUp();
            PowerUpPlayer();
            Destroy(gameObject, 0.05f);
        }


    }

    private void PowerUpPopUp()
    {
        powerUpCanvas.SetActive(true);
        powerUpCanvas.GetComponent<UIPowerUpCanvas>().UpdatePowerUpTutorial();
    }

    private void PowerUpPlayer()
    {
        switch (powerUpName)
        {
            case PowerUp.Dash:
                PlayerStatus.Instance.SetCanDash();
                break;
            case PowerUp.WallJump:
                PlayerStatus.Instance.SetCanWallJump();
                break;
            case PowerUp.DoubleJump:
                PlayerStatus.Instance.SetCanDoubleJump();
                break;
        }
    }
}
