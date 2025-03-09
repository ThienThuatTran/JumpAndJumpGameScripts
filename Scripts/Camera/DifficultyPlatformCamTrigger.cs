using Unity.Cinemachine;
using UnityEngine;

public class DifficultyPlatformCamTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera difficultCam;
    [SerializeField] private LayerMask cameraTriggerLayerMask;
    private CinemachineCamera mainVTCamera;

    public bool isTriggerEnter = false;
    public bool isTriggerExit = false;

    private void Start()
    {
        mainVTCamera = CameraManager.Instance.GetMainCamera();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            isTriggerEnter = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            isTriggerExit = true;
            
        }
    }

    private void LateUpdate()
    {
        if (PlayerStatus.Instance.isGrounded)
        {
            if (isTriggerEnter && isTriggerExit)
            {
                Collider2D difficultyPlatformZone = Physics2D.OverlapCircle(PlayerStatus.Instance.transform.position, 0.2f, cameraTriggerLayerMask);
                if (difficultyPlatformZone != null)
                {
                    isTriggerEnter = true;
                    isTriggerExit = false;
                }
                else
                {
                    isTriggerEnter = false;
                    isTriggerExit = true;
                }
            }
            else if (isTriggerEnter)
            {
                CameraTriggerEnter();
                isTriggerEnter = false;
            }

            else if (isTriggerExit)
            {
                CameraTriggerExit();
                isTriggerExit = false;
            }
        }
        

        
    }

    private void CameraTriggerEnter()
    {
        CameraManager.Instance.SwitchMainCamera(difficultCam, 1f);
        difficultCam.gameObject.SetActive(true);
    }

    private void CameraTriggerExit()
    {
        CameraManager.Instance.SwitchMainCamera(mainVTCamera, 2f);
        mainVTCamera.gameObject.SetActive(true);
        difficultCam.gameObject.SetActive(false);
    }

}
