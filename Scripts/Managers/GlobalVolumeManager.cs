using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeManager : MonoBehaviour
{
    private Volume globalVolume;
    private void Awake()
    {
        globalVolume = GetComponent<Volume>();
    }
}
