using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float waitTime = 1f;
    private List<Vector3> startTargets = new List<Vector3>();
    private List<Vector3> reverseTargets = new List<Vector3>();
    private int forward = 1;

    // Start is called before the first frame update
    void Start()
    {
        //movingPlatformRb = GetComponent<Rigidbody2D>();
        GetTargets();
        if (startTargets.Count > 0)
        {
            StartCoroutine(MovePlatform());
        }
    }
    private void OnEnable()
    {
        if (startTargets.Count > 0)
        {
            StartCoroutine(MovePlatform());
        }
    }
    private IEnumerator MovePlatform()
    {
        while (true)
        {
            
            List<Vector3> targets = new List<Vector3> ();
            if (forward == 1)
            {
                targets = startTargets;
            }
            else
            {
                targets = reverseTargets;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                
                Vector3 targetPosition =  targets[i];
               
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    //platformRb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
                    //Debug.Log(platformRb.velocity);
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(waitTime);
            }
            forward = -forward;

        }
    }
    private void GetTargets()
    {
        startTargets.Clear();
        reverseTargets.Clear();
        foreach (Transform child in transform)
        {
            //Debug.Log(position);
            startTargets.Add(child.position);
        }

        for (int i = startTargets.Count-2; i>=0; i--)
        {
            reverseTargets.Add(startTargets[i]);
        }
        //reverseTargets.Add(startTargets[transform.childCount-1]);

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
