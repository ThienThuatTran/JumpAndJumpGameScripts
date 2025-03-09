using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }

    [SerializeField] private bool canDoubleJump = false;
    [SerializeField] private bool canWallJump = false;
    [SerializeField] private bool canDash = false;

    [SerializeField] private float playerKnockedTime = 0.25f;
    [SerializeField] private int maxHealth = 5;
    public float towardRight;
    
    private bool isDead = false;
    private bool isKnocked = false;

    public bool isGrounded;

    private HealthSystem playerHealth;
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
        playerHealth = new HealthSystem(maxHealth);
    }

    public bool CanDoubleJump()
    {
        return canDoubleJump;
    }

    public void SetCanDoubleJump()
    {
        canDoubleJump = true;
    }
    public bool CanWallJump()
    {
        return canWallJump;
    }
    public void SetCanWallJump()
    {
        canWallJump = true;
    }

    public bool CanDash()
    {
        return canDash;
    }
    public void SetCanDash()
    {
        canDash = true;
    }

    public float GetKnockTime()
    {
        return playerKnockedTime;
    }

    public HealthSystem GetPlayerHealth()
    {
        return playerHealth;
    }

    public bool GetIsKnocked()
    {
        return isKnocked;
    }

    public void SetIsKnocked(bool knocked)
    {
        isKnocked = knocked;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
    public void SetIsDead()
    {
        isDead = true;
    }

    private void Update()
    {
        //towardRight = GetComponent<PlayerControllerHK>().towardRight;
    }
}
