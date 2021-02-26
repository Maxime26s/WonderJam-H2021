using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject player;
    BoxCollider2D bc2d;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
    }


    public void Use(Vector2 force, GameObject fleche)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = fleche.transform.position;
        bc2d.isTrigger = false;
        rb2d.isKinematic = false;
        /*
        rb2d = gameObject.AddComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.mass = 10;
        rb2d.constraints  = RigidbodyConstraints2D.FreezeRotation;
        */
        rb2d.AddForce(force/5);
    }
}
