using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIBrightnessSetting : MonoBehaviour
{
    [SerializeField] private VolumeProfile globalVolumeProfile;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TextMeshProUGUI brightnessText;
    private ColorAdjustments colorAdjustments;
    
    private void Start()
    {
        //UpdateBrightness();
        globalVolumeProfile.TryGet(out colorAdjustments);
        CheckBrightnessFromGlobalVolumeProfile();
        
    }
    public void UpdateBrightness()
    {
        if (colorAdjustments == null)
        {
            return;
        }
        float value = brightnessSlider.value;
        colorAdjustments.postExposure.value = value;
        brightnessText.text = Mathf.RoundToInt((value+2)/2 *100).ToString() + "%";
    }

    private void CheckBrightnessFromGlobalVolumeProfile()
    {
        if (colorAdjustments == null)
        {
            return;
        }
        float value = colorAdjustments.postExposure.value;
        brightnessSlider.value = value;
        brightnessText.text = Mathf.RoundToInt((value + 2) / 2 * 100).ToString() + "%";
    }
}
