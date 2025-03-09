using System;
using UnityEngine;

public class UIResetInput : MonoBehaviour
{
    
    

    public void ResetInput()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        InputManager.Instance.ResetInputRebind();
    }
}
