using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UIInputRebind : MonoBehaviour
{
    private static event EventHandler OnUpdateDisplay;
    [SerializeField] private TextMeshProUGUI inputName;
    [SerializeField] private InputAction inputAction;
    //[SerializeField] private string actionName;
    [SerializeField] private PlayerActionName playerActionName;
    //[SerializeField] private Key defaultKeyCode;
    private KeyControl key;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private int bindingIndex;

    private void Awake()
    {
        OnUpdateDisplay += UIInputRebind_OnUpdateDisplay;
    }

    private void UIInputRebind_OnUpdateDisplay(object sender, EventArgs e)
    {
        DisplayInputBinding();
    }

    private void Start()
    {

        /*
        inputAction = InputManager.Instance.GetPlayerInputAction().FindAction(actionName);
        //inputAction.GetBindingIndex();
            //InputManager.Instance.GetPlayerInputAction()
        key = new KeyControl();
        key.keyCode = defaultKeyCode;
        
        
        foreach (KeyControl keyControl in Keyboard.current.allKeys)
        {
            if (key.keyCode == keyControl.keyCode)
            {
                key = keyControl;
            }
        }
        
        inputName.text = key.keyCode.ToString();
        bindingIndex = inputAction.GetBindingIndexForControl(key);
        */
        if (!playerActionName.ToString().Contains("Movement"))
        {
            inputAction = InputManager.Instance.GetPlayerInputAction().FindAction(playerActionName.ToString());
        }
        else
        {
            inputAction = InputManager.Instance.GetPlayerInputAction().FindAction("Movement");
        }
        bindingIndex = PlayerPrefs.GetInt("Action" + playerActionName.ToString(), -1);
        DisplayInputBinding();
        InputManager.Instance.OnAllInputReset += InputManager_AllInputReset;
        //Debug.Log(Keyboard.current.GetChildControl<KeyControl>("a"));
    }

    private void InputManager_AllInputReset(object sender, System.EventArgs e)
    {
        ResetToDefault();
    }

    public void StartRebinding()
    {
        SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.clickSFX, transform, 1f);
        inputName.autoSizeTextContainer = true;
        inputName.text = "Waiting for input...";
        
        rebindingOperation = inputAction.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();

    }


    private void RebindComplete()
    {
        //Keyboard.current.TryGetChildControl
        rebindingOperation.Dispose();
        //Debug.Log(Keyboard.current.GetChildControl<KeyControl>(inputAction.bindings[bindingIndex].overridePath));
        
        //Debug.Log(inputAction.bindings[bindingIndex].overridePath);
        TestDuplicateInput();
        InputManager.Instance.SaveInputRebind();

        DisplayInputBinding();
    }
    private void DisplayInputBinding()
    {
        string newInputName = InputControlPath.ToHumanReadableString(
            inputAction.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        inputName.text = newInputName;
    }

    private void ResetToDefault()
    {
        inputAction.RemoveAllBindingOverrides();   
        DisplayInputBinding();
        InputManager.Instance.SaveInputRebind();
    }

    private void TestDuplicateInput()
    {
        string newInputName = InputControlPath.ToHumanReadableString(
           inputAction.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);


        newInputName = newInputName.Replace(" ", "");
        key = Keyboard.current.GetChildControl<KeyControl>(newInputName);

        InputActionAsset inputActions = InputManager.Instance.GetPlayerInputAction().asset;
        foreach (InputAction action in inputActions.actionMaps[0].actions)
        {
            if (action == inputAction)
            {
                continue;
            }
            
            int testBindingIndex = action.GetBindingIndexForControl(key);
            //Debug.Log(testBindingIndex);
            if (testBindingIndex != -1)
            {
                action.ApplyBindingOverride(testBindingIndex, "<Keyboard>/null");
                OnUpdateDisplay?.Invoke(this, EventArgs.Empty);
            }   
        }
    }

    
}
