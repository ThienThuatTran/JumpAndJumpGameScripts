using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFlyingTesting : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemySquare;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb2D;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2D = GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(this.UpdatePath), 0f, 0.2f);
        
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2D.position, target.position, OnPathFindingComplete);
        }
    }
    private void OnPathFindingComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
        }
    }

    private void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] -rb2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb2D.AddForce(force);
        //Debug.Log(force);

        float distance = Vector2.Distance(rb2D.position, (Vector2)path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemySquare.localScale = new Vector3(Mathf.Abs(enemySquare.localScale.x), enemySquare.localScale.y, enemySquare.localScale.z);
        }
        else if (force.x < 0.01f)
        {
            enemySquare.localScale = new Vector3(-Mathf.Abs(enemySquare.localScale.x), enemySquare.localScale.y, enemySquare.localScale.z);
        }
    }

}
