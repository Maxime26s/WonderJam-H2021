using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionEnemy : MonoBehaviour
{
    public EnemyAI ai;
    public float range = 20f;
    public float fov = 90f;
    public LayerMask layerMask;
    public Rigidbody2D rb;
    public float lastHitTime;

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
        for(int i = 0; i < nbRay; i++)
        {
            float angle = startAngle + i * fov * Mathf.Deg2Rad / nbRay;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), range, layerMask);
            Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))*range, Color.green);
            if (hit.collider != null && hit.collider.tag == "Player")
                Chase(hit);
        }
        if(ai.chasing && Time.time - lastHitTime > 3f)
        {
            ai.target = ai.route[0];
            ai.chasing = false;
        }
    }
    void Chase(RaycastHit2D hit)
    {
        ai.chasing = true;
        lastHitTime = Time.time;
        ai.target = hit.collider.transform;
    }
}
