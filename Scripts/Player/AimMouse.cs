using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMouse : MonoBehaviour
{
    [SerializeField] private Camera fPCamera;
    [SerializeField] private Transform bowTransform;
    [SerializeField] private Transform bowMainTransform;
    private PlayerControllerHK playerController;
    private Vector3 shootDirection = new Vector3();
    private float arrowAngle;
    private Vector3 mousePosition;
    private Vector3 bowMainStartPosition;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }

    private void GameManager_OnRespawnPlayer(object sender, System.EventArgs e)
    {
        
    }

    void Start()
    {
        fPCamera = CameraManager.Instance.followCamera;
        //GameManager.Instance.OnRespawnPlayer += GameManager_OnRespawnPlayer;
        playerController = GetComponent<PlayerControllerHK>();
        bowMainStartPosition = bowMainTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if ( fPCamera == null)
        {
            return;
        }
        switch (playerController.bowMode)
        {
            case Bow.BowMode.Arrow:
                bowMainTransform.localPosition = bowMainStartPosition;
                GetMouseToAim();
                AimTarget(GetTargetToAim(shootDirection));
                break;
            case Bow.BowMode.Grappling:
                bowMainTransform.localPosition = new Vector3(0, 1, 0);
                GetMouseToAim();
                AimTarget(GetTargetToAim(shootDirection));
                if (playerController.isGraplingHook)
                {
                    AimTarget(GetTargetToAim((Vector3)playerController.graplingPoint - bowTransform.position));
                }
                break;
            
            default:
                break;
        }*/
    }

    private void AimTarget(float targetAngle)
    {
        bowTransform.eulerAngles = new Vector3(0, 0, targetAngle);
    }
    private void GetMouseToAim()
    {
        mousePosition = (fPCamera.ScreenToWorldPoint(Input.mousePosition));

        shootDirection = (mousePosition - bowTransform.transform.position).normalized;
    }
    private float GetTargetToAim(Vector3 shootDir)
    { 
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        float toward = transform.localScale.x / Mathf.Abs(transform.localScale.x);
        if (toward < 0) angle += 180;
        arrowAngle = angle;
        return angle;
    }
    public Vector3 ShootDirection()
    {
        return shootDirection;
    }
    public float ArrowAngle()
    {
        return arrowAngle;
    }
    public Vector3 MousePosition()
    {
        return mousePosition;
    }

}
