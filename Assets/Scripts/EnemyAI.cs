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
    public float maxSpeedChase;
    public GameObject light;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject shadow;

    public float speed = 200f, maxSpeed = 100f;
    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    //bool reachedEndOfPath = false;

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
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);

        if (direction.magnitude > 0.5)
        {
            direction = direction.normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = chasing ? direction * maxSpeedChase : rb.velocity.normalized * maxSpeed;

            if (!chasing)
                light.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x) - 90);
            else
            {
                Vector2 dir = (Vector2)target.position - rb.position;
                light.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) - 90);
            }

            if (light.transform.rotation.eulerAngles.z > 270 || light.transform.rotation.eulerAngles.z < 90)
            {
                animator.SetBool("Behind", true);
                shadow.SetActive(true);
            }
            else
            {
                animator.SetBool("Behind", false);
                shadow.SetActive(false);
            }  

            if (light.transform.rotation.eulerAngles.z > 0 && light.transform.rotation.eulerAngles.z < 180)
            {
                spriteRenderer.flipX = true;
                shadow.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                spriteRenderer.flipX = false;
                shadow.transform.localScale = new Vector3(1, 1, 1);
            }
        }

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
