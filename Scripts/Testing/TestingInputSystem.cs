using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    private bool isHold = false;
    /*
    PlayerInputAction inputAction;
    private void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.Enable();
        inputAction.Player.Movement.performed += Movement_performed;
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        Debug.Log(inputAction.Player.Movement.ReadValue<Vector2>());
    }
    
    public void OnMove(InputAction.CallbackContext obj)
    {
        Debug.Log("movement");
    }
    */
    public void OnTest(InputAction.CallbackContext obj)
    {
        if (obj.phase == InputActionPhase.Started)
            isHold = false;
        if (obj.phase == InputActionPhase.Performed)
        {
            isHold = true;
            Debug.Log("isHolding");
        }
        if (obj.phase == InputActionPhase.Canceled)
            if (!isHold) { Debug.Log("Press"); }

                
    }
}
