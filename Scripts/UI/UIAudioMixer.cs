using UnityEngine;
using UnityEngine.Audio;

public class UIAudioMixer : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sFXVolume", Mathf.Log10(volume)*20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }
}
