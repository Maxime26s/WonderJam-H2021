using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float rayon, tempsMaxCharge, force, forceMultiplier;
    public GameObject fleche;
    public bool charge;
    public float chargeStartTime, pourcent;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        fleche.transform.localPosition = direction * rayon;
        fleche.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * Mathf.Atan2(direction.y,direction.x) - 90);
        pourcent = (Time.time - chargeStartTime) / tempsMaxCharge;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charge = true;
            chargeStartTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            charge = false;
            float chargeEndTime = Time.time;
            float chargeTime = chargeEndTime - chargeStartTime;
            float chargeForce = chargeTime >= tempsMaxCharge ? 1 : chargeTime / tempsMaxCharge;
            rb.AddForce(direction * chargeForce * force * forceMultiplier);
        }
    }
}
