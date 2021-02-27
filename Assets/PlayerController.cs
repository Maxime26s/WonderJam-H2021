using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public PlayerInput playerInput;

    public float rayon, tempsMaxCharge, force, forceMultiplier, offsetFleche, flecheMinScale, flecheMaxScale;
    public GameObject fleche, collisionParticles1, collisionParticles2, deathParticles;
    public bool isCharging;
    public float chargeStartTime, pourcent;
    public Rigidbody2D rb;
    public SpriteRenderer flecheSpriteRenderer;
    public PlayerEnum playerNum = PlayerEnum.One;
    Vector2 direction;

    //Collect
    private CircleCollider2D cc2d;
    private bool holding = false;
    private GameObject objectHolding;
    string objType;
    bool throwable;

    private void Awake()
    {
        //fleche.SetActive(true);
        //flecheSpriteRenderer = fleche.GetComponent<SpriteRenderer>();
        cc2d = GetComponent<CircleCollider2D>();
        flecheSpriteRenderer = fleche.GetComponent<SpriteRenderer>();
        //fleche.SetActive(false);
        try
        {
            GameInfo.Instance.AddPlayer(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log("fail");
        }
    }

    private void Update()
    {
        if (isCharging)
        {
            pourcent = (Time.time - chargeStartTime) / tempsMaxCharge;
            pourcent = pourcent >= 1 ? 1 : pourcent;
            flecheSpriteRenderer.color = Color.Lerp(Color.green, Color.red, pourcent);
            fleche.transform.localScale = new Vector3(flecheMinScale + (flecheMaxScale - flecheMinScale) * pourcent, fleche.transform.localScale.y, fleche.transform.localScale.x);
        }
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!isCharging && ctx.performed)
        {
            isCharging = true;
            chargeStartTime = Time.time;
        }
    }

    public void Release(InputAction.CallbackContext ctx)
    {
        if (isCharging && ctx.performed)
        {
            isCharging = false;
            rb.AddForce(direction * pourcent * force * forceMultiplier);

            flecheSpriteRenderer.color = Color.white;
            pourcent = 0;
            fleche.transform.localScale = new Vector3(flecheMinScale, fleche.transform.localScale.y, fleche.transform.localScale.z);
        }
    }

    public void Use(InputAction.CallbackContext ctx)
    {
        if (!isCharging && ctx.performed)
        {
            if (holding && objectHolding.GetComponent<PowerUp>().throwable)
            {
                isCharging = true;
                chargeStartTime = Time.time;
            }
        }
    }

    public void UseRelease(InputAction.CallbackContext ctx)
    {
        if (isCharging && ctx.performed)
        {
            if (holding && objectHolding.GetComponent<PowerUp>().throwable)
            {
                isCharging = false;
                switch (objType)
                {
                    case "box":
                        objectHolding.GetComponent<Box>().Use(direction * pourcent * force * forceMultiplier, fleche);
                        objectHolding.GetComponent<PowerUp>().SetUsed();
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

    public void Move(InputAction.CallbackContext ctx)
    {
        /*
        float oldAngle = transform.rotation.z;
        float newAngle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - offsetFleche;
        Mathf.Lerp(oldAngle, newAngle, 0.1f);
        */
        direction = ctx.ReadValue<Vector2>();

        if (direction.magnitude < 0.3)
            fleche.SetActive(false);
        else
        {
            fleche.SetActive(true);
        }

        direction = direction.normalized;

        fleche.transform.localPosition = direction * rayon;
        fleche.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - offsetFleche);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            //TODO: Drop items

            //TODO: Lose powerups

            //TODO: Lose points

            Instantiate(deathParticles, transform.position, Quaternion.identity);

            switch (playerNum)
            {
                case PlayerEnum.One:
                    transform.position = GameManager.Instance.spawn1.transform.position;
                    break;
                case PlayerEnum.Two:
                    transform.position = GameManager.Instance.spawn2.transform.position;
                    break;
                default:
                    transform.position = Vector3.zero;
                    Debug.Log("GIVE THIS PLAYER A NUMBER!!!");
                    break;
            }
        }
        else
        {
            switch (playerNum)
            {
                case PlayerEnum.One:
                    Instantiate(collisionParticles1, collision.GetContact(0).point, Quaternion.identity);
                    break;
                case PlayerEnum.Two:
                    Instantiate(collisionParticles2, collision.GetContact(0).point, Quaternion.identity);
                    break;
            }
        }
    }

    public enum PlayerEnum
    {
        One,
        Two,
    }
}
