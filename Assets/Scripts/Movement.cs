using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float rayon, tempsMaxCharge, force, forceMultiplier, offsetFleche, flecheMinScale, flecheMaxScale;
    public GameObject fleche;
    public bool isCharging;
    public float chargeStartTime, pourcent;
    public Rigidbody2D rb;
    SpriteRenderer flecheSpriteRenderer;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode move;
    public KeyCode usePU;

    public KeyCode upController;
    public KeyCode downController;
    public KeyCode leftController;
    public KeyCode rightController;
    public KeyCode moveController;
    public KeyCode usePUController;

    // Start is called before the first frame update
    void Start()
    {
        flecheSpriteRenderer = fleche.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (direction.magnitude < 0.3)
            fleche.SetActive(false);
        else
        {
            fleche.SetActive(true);
        }

        direction = direction.normalized;

        fleche.transform.localPosition = direction * rayon;
        fleche.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - offsetFleche);
        if (isCharging)
        {
            pourcent = (Time.time - chargeStartTime) / tempsMaxCharge;
            pourcent = pourcent >= 1 ? 1 : pourcent;
            flecheSpriteRenderer.color = Color.Lerp(Color.green, Color.red, pourcent);
            fleche.transform.localScale = new Vector3(flecheMinScale + (flecheMaxScale - flecheMinScale) * pourcent, fleche.transform.localScale.y, fleche.transform.localScale.x);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeStartTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isCharging = false;
            rb.AddForce(direction * pourcent * force * forceMultiplier);

            flecheSpriteRenderer.color = Color.white;
            pourcent = 0;
            fleche.transform.localScale = new Vector3(flecheMinScale, fleche.transform.localScale.y, fleche.transform.localScale.z);
        }
    }
}
