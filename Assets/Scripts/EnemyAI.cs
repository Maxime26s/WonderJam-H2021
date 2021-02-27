using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public List<Transform> route = new List<Transform>();
    public int routeIndex = 0;
    public bool chasing = false;
    public float distanceminmaxthing;

    public float speed = 200f, maxSpeed = 100f;
    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = route[0];
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = chasing ? rb.velocity.normalized * maxSpeed : rb.velocity.normalized * maxSpeed / 2f;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x));

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if ((!chasing && !GameManager.Instance.isSuperAlert) && Vector3.Distance(route[routeIndex].position, transform.position) < distanceminmaxthing)
        {
            routeIndex = routeIndex + 1 < route.Count ? routeIndex + 1 : 0;
            target = route[routeIndex];
        }
    }
}
