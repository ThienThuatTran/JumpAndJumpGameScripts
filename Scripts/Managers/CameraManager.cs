using System.Xml.Serialization;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public Camera followCamera;

    [SerializeField] private CinemachineCamera upCamera;
    [SerializeField] private CinemachineCamera downCamera;
    [SerializeField] private CinemachineCamera mainVTCamera;
    private CinemachineBrain mainCameraBrain;

    private bool isFirstCameraLook = true;
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
            //DontDestroyOnLoad(gameObject);
        }

    }
    private void Start()
    {
        mainCameraBrain = followCamera.GetComponent<CinemachineBrain>();
        if (mainCameraBrain == null)
        {
            Debug.Log("null");
        }
        
    }
    public void LookUp()
    {
        upCamera.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition.x =
            mainVTCamera.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition.x;

        mainVTCamera.gameObject.SetActive(false);
        upCamera.gameObject.SetActive(true);
        mainCameraBrain.DefaultBlend.Time = 1f;
    }

    public void LookDown()
    {
        downCamera.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition.x =
            mainVTCamera.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition.x;

        mainVTCamera.gameObject.SetActive(false);
        downCamera.gameObject.SetActive(true);
        mainCameraBrain.DefaultBlend.Time = 1f;
    }

    public void ReturnNormalCamera()
    {
        upCamera.gameObject.SetActive(false);
        downCamera.gameObject.SetActive(false);
        mainVTCamera.gameObject.SetActive(true);
        mainCameraBrain.DefaultBlend.Time = 0.4f;

    }
    private void Update()
    {
        
        /*
        if (InputManager.Instance.GetPlayerInputAction().Player.LookDown.WasPressedThisFrame())
        {
            if (isFirstCameraLook)
            {
                LookDown();
                isFirstCameraLook = false;
            }

            
        }
        if (InputManager.Instance.GetPlayerInputAction().Player.LookDown.WasReleasedThisFrame())
        {
            ReturnNormalCamera();
            isFirstCameraLook = true;
        }


        if (InputManager.Instance.GetPlayerInputAction().Player.LookUp.WasPressedThisFrame())
        {
            if (isFirstCameraLook)
            {
                LookUp();
                isFirstCameraLook = false;
            }
        }
        if (InputManager.Instance.GetPlayerInputAction().Player.LookUp.WasReleasedThisFrame())
        {
            ReturnNormalCamera();
            isFirstCameraLook = true;
        }
        */
    }

    public CinemachineCamera GetMainCamera()
    {
        return mainVTCamera;
    }
    public CinemachineBrain GetMainBrain()
    {
        return mainCameraBrain;
    }

    public void SwitchMainCamera(CinemachineCamera newMainCineCamera, float blendTime)
    {
        mainCameraBrain.DefaultBlend.Time = blendTime;
        mainVTCamera.gameObject.SetActive(false);
        mainVTCamera = newMainCineCamera;
        
    }
    private void OnDestroy()
    {
        //Destroy(gameObject);
    }
}
