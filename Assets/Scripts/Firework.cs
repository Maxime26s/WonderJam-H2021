using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    public GameObject player;
    BoxCollider2D bc2d;
    Rigidbody2D rb2d;
    Vector2 forceV = new Vector2(0,0);
    public GameObject collisionParticles;

    Vector2 originOfExplode;
    public float radius = 5;
    public float forceMultiplier = 5;

    // Start is called before the first frame update
    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        bc2d.isTrigger = true;
    }

    private void OnEnable()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        bc2d.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.AddForce(forceV);
    }

    public void Use(Vector2 force, GameObject fleche)
    {
        gameObject.SetActive(true);
        transform.position = fleche.transform.position;
        transform.rotation = fleche.transform.rotation;
        bc2d.isTrigger = false;
        rb2d.isKinematic = false;

        forceV = force;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        originOfExplode = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(originOfExplode, radius);

        foreach (Collider2D col in colliders)
        {
            // the force will be a vector with a direction from origin to collider's position and with a length of 'forceMultiplier'
            Vector2 force = (new Vector2(col.transform.position.x, col.transform.position.y) - originOfExplode) * forceMultiplier;
            
            Rigidbody2D rb = col.transform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(force);
            }
        }
        Instantiate(collisionParticles, collision.GetContact(0).point, Quaternion.identity);
        Debug.Log("oy");
        player.GetComponent<PlayerController>().StartRespawn(gameObject);
        //Destroy(gameObject);
    }
}
