using Unity.Cinemachine;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followCamera;
    private void Start()
    {
        //GameManager.Instance.OnRespawnPlayer += GameManager_OnRespawnPlayer;
        //followCamera = GetComponent<CinemachineCamera>();
    }

    private void GameManager_OnRespawnPlayer(object sender, System.EventArgs e)
    {
        followCamera.Follow = Player.Instance.transform;
    }
}
