using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayCollision : MonoBehaviour
{
    /*
    private PlayerInputAction playerInputAction2;
    private PlatformEffector2D platformEffector2D;
    private float waitTime = 0.3f;
    private float waitTimer = -1;
    // Start is called before the first frame update
    void Awake()
    {
        platformEffector2D = GetComponent<PlatformEffector2D>();
        playerInputAction2 = new PlayerInputAction();
        
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("jump2");
    }

    private void OneWayCollision_performed(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            waitTimer = waitTime;
            Debug.Log("Hi");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer <0)
        {
            platformEffector2D.rotationalOffset = 0;
        }
        else { platformEffector2D.rotationalOffset = 180; }
        if (waitTimer>-1) waitTimer -= Time.deltaTime;
    }
    */


}
