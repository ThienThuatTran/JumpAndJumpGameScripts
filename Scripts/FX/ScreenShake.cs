using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private float damageShakeForce = 2f;
    [SerializeField] private float enemyDefeatShakeForce = 1f;
    public static ScreenShake Instance;
    private CinemachineImpulseSource impulseSource;
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
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Shake(float force)
    {
        impulseSource.GenerateImpulse(force);
    }

    public void DamageScreenShake()
    {
        Shake(damageShakeForce);
    }

    public void EnemyDefeatScreenShake()
    {
        Shake(enemyDefeatShakeForce);
    }
}
