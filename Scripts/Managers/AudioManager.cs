using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string[] volumeNameArray;


    private void Start()
    {
        foreach (var volumeName in volumeNameArray)
        {
            float volume = PlayerPrefs.GetFloat(volumeName, 0.5f);
            audioMixer.SetFloat(volumeName, 20* Mathf.Log10(volume));
        }
    }
}
