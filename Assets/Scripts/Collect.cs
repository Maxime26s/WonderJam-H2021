using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private CircleCollider2D cc2d;
    private bool holding = false;
    private GameObject objectHolding;
    void Start()
    {
        cc2d = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && holding){
            //objectHolding.GetComponent<>().Use();
            objectHolding = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        holding = true;
        objectHolding = collision.gameObject;
    }


}
