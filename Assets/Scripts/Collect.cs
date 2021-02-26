using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private CircleCollider2D cc2d;
    private bool holding = false;
    private GameObject objectHolding;
    string objType;
    bool throwable;

    //Pour la fleche
    public float rayon, tempsMaxCharge, force, forceMultiplier, offsetFleche, flecheMinScale, flecheMaxScale;
    public GameObject fleche;
    public bool isCharging;
    public float chargeStartTime, pourcent;
    SpriteRenderer flecheSpriteRenderer;


    void Start()
    {
        cc2d = GetComponent<CircleCollider2D>();
        flecheSpriteRenderer = fleche.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(holding && objectHolding.GetComponent<PowerUp>().throwable)
        {
            Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (isCharging)
            {
                pourcent = (Time.time - chargeStartTime) / tempsMaxCharge;
                pourcent = pourcent >= 1 ? 1 : pourcent;
                flecheSpriteRenderer.color = Color.Lerp(Color.green, Color.red, pourcent);
                fleche.transform.localScale = new Vector3(flecheMinScale + (flecheMaxScale - flecheMinScale) * pourcent, fleche.transform.localScale.y, fleche.transform.localScale.x);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                isCharging = true;
                chargeStartTime = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                isCharging = false;
                switch (objType)
                {
                    case "box":
                        objectHolding.GetComponent<Box>().Use(direction * pourcent * force * forceMultiplier, fleche); 
                        break;
                }
                flecheSpriteRenderer.color = Color.white;
                pourcent = 0;
                fleche.transform.localScale = new Vector3(flecheMinScale, fleche.transform.localScale.y, fleche.transform.localScale.z);
                objectHolding = null;
                objType = "";
                holding = false;
                throwable = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            holding = true;
            objectHolding = collision.gameObject;
            PowerUp pw = objectHolding.GetComponent<PowerUp>();
            if (pw != null)
            {
                throwable = pw.throwable;
                objType = pw.type;
                pw.OnCollection();
                switch (objType)
                {
                    case "box":
                        objectHolding.GetComponent<Box>().player = gameObject;
                        break;
                }
            }
        }
    }
}
