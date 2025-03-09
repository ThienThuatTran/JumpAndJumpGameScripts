using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private Transform trackedObject; 
    [SerializeField] private float updateSpeed = 3; 
    [SerializeField] private Vector2 trackingOffset; 
    private Vector3 offset;

    private void Start()
    {
        offset = (Vector3)trackingOffset; 

        offset.z = transform.position.z - trackedObject.position.z;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, trackedObject.position + offset, updateSpeed * Time.deltaTime);
    }
}
