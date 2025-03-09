using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UpdateAudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string volumeName;
    private void Start()
    {
        CheckAudioVolumeInSaveFile();
    }
    public void UpdateVolume()
    {
        float volume = volumeSlider.value;
        SetVolume(volumeName, volume);
        volumeText.text = Mathf.RoundToInt(volume*100f).ToString() + "%";
        PlayerPrefs.SetFloat(volumeName, volume);
    }
    private void SetVolume(string volumeName, float volume)
    {
        audioMixer.SetFloat(volumeName, Mathf.Log10(volume) * 20f);
        
    }

    private void CheckAudioVolumeInSaveFile()
    {
        float volume = PlayerPrefs.GetFloat(volumeName, 0.5f);
        volumeSlider.value = volume;
        SetVolume(volumeName, volume);
        volumeText.text = Mathf.RoundToInt(volume * 100f).ToString() + "%";
    }
}
