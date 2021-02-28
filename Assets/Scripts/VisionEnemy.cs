using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VisionEnemy : MonoBehaviour
{
    public EnemyAI ai;
    public float range = 20f;
    public float fov = 90f;
    public LayerMask layerMask;
    public Rigidbody2D rb;
    public float lastHitTime;
    public Color colorIdle, colorFound;
    public Light2D lightVision;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = rb.velocity;
        float startAngle = (Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - fov / 2) * Mathf.Deg2Rad;
        int nbRay = (int)Mathf.Ceil(fov);
        float smallestDistance = Mathf.Infinity;
        Transform closest = null;

        for (int i = 0; i < nbRay; i++)
        {
            float angle = startAngle + i * fov * Mathf.Deg2Rad / nbRay;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), range, layerMask);
            //Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * range, Color.green);
            if (hit.collider != null && hit.collider.tag == "Player" && !hit.collider.gameObject.GetComponent<PlayerController>().invisi)
            {
                float currentDist = Vector3.Distance(hit.point, rb.position);
                if (i == 0 || currentDist < smallestDistance)
                {
                    closest = hit.collider.transform;
                    smallestDistance = currentDist;
                }
            }
        }
        if(closest != null)
            Chase(closest);
        if ((ai.chasing && Time.time - lastHitTime > 3f) || (ai.chasing && ai.target.GetComponent<PlayerController>().invisi))
        {
            ai.target = ai.route[ai.routeIndex];
            ai.chasing = false;
            lightVision.color = colorIdle;
        }
    }
    void Chase(Transform hitTransform)
    {
        ai.chasing = true;
        lastHitTime = Time.time;
        ai.target = hitTransform;
        lightVision.color = colorFound;
    }
}
