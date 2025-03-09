using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public event EventHandler OnAllInputReset;
    public static InputManager Instance;
    [SerializeField] private Key[] defaultKeys;

    [SerializeField] private ActionsWithKey[] playerActionsWithKeys;

    private PlayerInputAction playerInputAction;
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
        }
        
        playerInputAction = new PlayerInputAction();
        
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Action" + playerActionsWithKeys[0].playerActionName.ToString(), -1) == -1)
        {
            playerInputAction.RemoveAllBindingOverrides();
            CheckDefaultBindingIndex();
            SaveInputRebind();
            
        }
        else
        {
            LoadInputRebind();
        }
    }


    public void SetPlayerMapsInputAction()
    {
        playerInputAction.UI.Disable();
        playerInputAction.Player.Enable();
    }

    public void SetUIMapsInputAction()
    {
        playerInputAction.Player.Disable();
        playerInputAction.UI.Enable();

        
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputAction;
    }
    private void OnDestroy()
    {
        playerInputAction.Disable();
    }

    public void ResetInputRebind()
    {
        OnAllInputReset?.Invoke(this, EventArgs.Empty);
    }
    public void SaveInputRebind()
    {
        var rebinds = playerInputAction.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        Debug.Log("save");
        // Restore player rebinds from PlayerPrefs (removes all existing
        // overrides on the actions; pass `false` for second argument
        // in case you want to prevent that).

    }

    public void LoadInputRebind()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        playerInputAction.LoadBindingOverridesFromJson(rebinds);
    }

    private void CheckDefaultBindingIndex()
    {
        for (int i = 0; i <= playerActionsWithKeys.Length-1; i++)
        {
            InputAction inputAction;
            if (!playerActionsWithKeys[i].playerActionName.ToString().Contains("Movement")) 
            {
                inputAction = playerInputAction.FindAction(playerActionsWithKeys[i].playerActionName.ToString());
            }
            else
            {
                inputAction = playerInputAction.FindAction("Movement");
            }
            //Debug.Log(playerActionsWithKeys[i].playerActionName.ToString());
                
            KeyControl key = new KeyControl();
            key.keyCode = playerActionsWithKeys[i].actionKey;


            foreach (KeyControl keyControl in Keyboard.current.allKeys)
            {
                if (key.keyCode == keyControl.keyCode)
                {
                    key = keyControl;
                    break;
                }
            }
            int bindingIndex = inputAction.GetBindingIndexForControl(key);

            PlayerPrefs.SetInt("Action" + playerActionsWithKeys[i].playerActionName.ToString(), bindingIndex);
        }
    }

}

public enum PlayerActionName
{
    LookUp,
    LookDown,
    MovementLeft,
    MovementRight,
    Jump,
    Dash,
    MenuOpen
}

[Serializable]
public struct ActionsWithKey
{
    public PlayerActionName playerActionName;
    public Key actionKey;
}
