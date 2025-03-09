using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public static event EventHandler OnBackgroundScroll;
    [SerializeField] private Vector2 parallaxEffectMultifier;
    [SerializeField] private bool isHorizontalLoop = true;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    //private Vector3 startCameraPos;
    //private float length;
    private float textureUnitSize;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
        //startCameraPos = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        //length = sprite.bounds.size.x;


        textureUnitSize = texture.width / sprite.pixelsPerUnit;
    }


    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x *parallaxEffectMultifier.x, deltaMovement.y * parallaxEffectMultifier.y);

        lastCameraPosition =cameraTransform.position;

       // float deltaCameraDistance = cameraTransform.position.x - startCameraPos.x;
        
        /*
        if (deltaCameraDistance*(1-parallaxEffectMultifier.x) >=length)
        {
           
            transform.position += new Vector3(length, 0, 0);
            startCameraPos = cameraTransform.position;
        }
        else if (deltaCameraDistance * (1 - parallaxEffectMultifier.x) <= -length)
        {
            transform.position -= new Vector3(length, 0, 0);
            startCameraPos = cameraTransform.position;
        }
        */
        if (isHorizontalLoop)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSize)
            {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSize;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
        
        
    }

}
