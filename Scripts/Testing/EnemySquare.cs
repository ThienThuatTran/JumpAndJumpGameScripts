using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySquare : MonoBehaviour
{
    public AIPath aIPath;
    private float lastXPosition;
    private float flipTimer = 0;
    [SerializeField] private float flipTime = 0.2f;
    void Update()
    {
        flipTimer += Time.deltaTime;
        if (transform.position.x > lastXPosition && transform.localScale.x > 0 && flipTimer > flipTime)
        {
            flipTimer = 0;
            transform.localScale = new Vector3(-Mathf.Abs( transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < lastXPosition && transform.localScale.x < 0 && flipTimer > flipTime)
        {
            flipTimer = 0;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        lastXPosition = transform.position.x;
        
    }
}
