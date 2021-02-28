using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

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
    public GameObject ball;
    public RuntimeAnimatorController ballController, playerController;
    public Color color2;
    public float invincibleRespawnTime = 2f;
    public ColObjectives colObjectives;
    Vector2 direction;

    public float invisibleTime;
    public bool invisi;

    //Collect
    private CircleCollider2D cc2d;
    public bool holding = false;
    public GameObject objectHolding;
    string objType;
    bool throwable;
    private Animator ballAnimator;
    private Animator playerAnimator;
    bool invincible = false;

    private void Awake()
    {
        cc2d = GetComponent<CircleCollider2D>();
        flecheSpriteRenderer = fleche.GetComponent<SpriteRenderer>();
        ballAnimator = ball.GetComponent<Animator>();
        playerAnimator = GetComponent<Animator>();

        IEnumerator WaitAndSetupPlayer2()
        {
            yield return new WaitForSeconds(0.1f);
            if(playerNum == PlayerEnum.Two)
            {
                ballAnimator.runtimeAnimatorController = ballController;
                playerAnimator.runtimeAnimatorController = playerController;
                GetComponentInChildren<Light2D>().color = color2;
            }
        }

        StartCoroutine(WaitAndSetupPlayer2());

        try
        {
            GameInfo.Instance.AddPlayer(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        GameManager.Instance.UpdateUI(gameObject);
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

        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90);
        if (rb.velocity.magnitude > 0)
            ball.transform.rotation = rotation;

        if (rb.velocity.x >= 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        ballAnimator.speed = rb.velocity.magnitude / 2;

        if (rb.velocity.magnitude > 0.1)
        {
            playerAnimator.SetBool("Running", true);
            playerAnimator.speed = rb.velocity.magnitude / 2;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            playerAnimator.speed = 1;
            playerAnimator.SetBool("Running", false);
        }
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!isCharging && ctx.performed)
        {
            isCharging = true;
            chargeStartTime = Time.time;
        }
        GameManager.Instance.UpdateUI(gameObject);
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
        GameManager.Instance.UpdateUI(gameObject);
    }

    public void Use(InputAction.CallbackContext ctx)
    {
        try
        {
            if (!GameInfo.Instance.started)
            {
                GameInfo.Instance.started = true;
                GameInfo.Instance.lobbyScript.StartGame();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        if (!isCharging && ctx.performed)
        {
            if (holding && objectHolding.GetComponent<PowerUp>().throwable)
            {
                isCharging = true;
                chargeStartTime = Time.time;
            }
        }
        GameManager.Instance.UpdateUI(gameObject);
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
                        objectHolding.GetComponent<Box>().Use(direction * 1 * force * forceMultiplier, fleche);
                        objectHolding.GetComponent<PowerUp>().SetUsed();
                        break;
                    case "firework":
                        objectHolding.GetComponent<Firework>().Use(direction * 1 * force * forceMultiplier, fleche);
                        objectHolding.GetComponent<PowerUp>().SetUsed();
                        break;
                    default:
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
        GameManager.Instance.UpdateUI(gameObject);
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
        if (collision.gameObject.CompareTag("PowerUp") && !holding)
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
                    case "firework":
                        break;
                    case "invisible":
                        Color temp = gameObject.GetComponent<SpriteRenderer>().color;
                        temp.a = 0.2f;
                        gameObject.GetComponent<SpriteRenderer>().color = temp;
                        temp = ball.GetComponent<SpriteRenderer>().color;
                        temp.a = 0.2f;
                        ball.GetComponent<SpriteRenderer>().color=temp;
                        StartCoroutine(invisible());
                        break;

                }
            }
        }
        GameManager.Instance.UpdateUI(gameObject);
    }
    IEnumerator invisible()
    {
        invisi = true;
        yield return new WaitForSeconds(invisibleTime);
        invisi = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy" && !invincible)
        {
            colObjectives.objective.transform.position = transform.position;

            objectHolding = null;
            holding = false;

            Instantiate(deathParticles, transform.position, Quaternion.identity);

            IEnumerator Respawning()
            {
                invincible = true;
                yield return new WaitForSeconds(invincibleRespawnTime);
                invincible = false;
            }

            StartCoroutine(Respawning());

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

            colObjectives.objective.SetActive(true);
            colObjectives.holdingObjective = false;
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
        GameManager.Instance.UpdateUI(gameObject);
    }
}

public enum PlayerEnum
{
    One,
    Two,
}
