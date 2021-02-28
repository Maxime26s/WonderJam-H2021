using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject street;
    public GameObject building;
    public GameObject sky;
    public GameObject player;
    public GameObject text;
    public GameObject museum;
    public GameObject raccoon;
    public GameObject moneyBags;
    public GameObject ball;
    public GameObject machine;
    public GameObject black;

    public float streetMoveSpeed;
    public float buildingMoveSpeed;
    public float skyMoveSpeed;
    public float textMoveSpeed;

    public LevelLoader loader;

    private Vector3 initialStreet;
    private Vector3 initialBuilding;
    private Vector3 initialSky;
    private bool moving;
    private bool over = false, done = false;

    // Start is called before the first frame update
    void Start()
    {
        initialStreet = street.transform.position;
        initialBuilding = building.transform.position;
        initialSky = sky.transform.position;

        moving = true;

        StartCoroutine("MoveText");
        StartCoroutine("MoveStreet");
        StartCoroutine("MoveBuildings");
        StartCoroutine("MoveSky");
        StartCoroutine("MoveMuseum");
        StartCoroutine("MoveRaccoon");
        StartCoroutine("MoveMachine");
        StartCoroutine("MoveMoneyBags");
        StartCoroutine("MoveBall");
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<Animator>().SetBool("Running", true);

        if (over && !done)
        {
            done = true;
            loader.LoadNextIndexAdditive();
        }
    }

    public IEnumerator MoveStreet()
    {
        for (float i = 0f; i > -18f; i -= streetMoveSpeed * Time.deltaTime)
        {
            if (moving)
                street.transform.position = new Vector3(street.transform.position.x - streetMoveSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }

        street.transform.position = initialStreet;

        if (moving)
            StartCoroutine("MoveStreet");
    }

    public IEnumerator MoveBuildings()
    {
        for (float i = 0f; i > -16f; i -= buildingMoveSpeed * Time.deltaTime)
        {
            if (moving)
                building.transform.position = new Vector3(building.transform.position.x - buildingMoveSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }

        building.transform.position = initialBuilding;

        if(moving)
            StartCoroutine("MoveBuildings");
    }

    public IEnumerator MoveSky()
    {
        for (float i = 0f; i > -16f; i -= skyMoveSpeed * Time.deltaTime)
        {
            if (moving)
                sky.transform.position = new Vector3(sky.transform.position.x - skyMoveSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }

        sky.transform.position = initialSky;

        if (moving)
            StartCoroutine("MoveSky");
    }

    public IEnumerator MoveText()
    {
        for (float i = 1083f; i > -2220f; i -= textMoveSpeed * Time.deltaTime)
        {
            text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, text.GetComponent<RectTransform>().anchoredPosition.y + textMoveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        StartCoroutine("JumpFar");
    }

    public IEnumerator MoveMuseum()
    {
        yield return new WaitForSeconds(2f);
        for (float i = 0f; i > -24f; i -= streetMoveSpeed * Time.deltaTime)
        {
            museum.transform.position = new Vector3(museum.transform.position.x - streetMoveSpeed * Time.deltaTime, museum.transform.position.y, 0);
            yield return null;
        }
    }

    public IEnumerator MoveRaccoon()
    {
        yield return new WaitForSeconds(15f);
        for (float i = 0f; i > -24f; i -= streetMoveSpeed * Time.deltaTime)
        {
            raccoon.transform.position = new Vector3(raccoon.transform.position.x - streetMoveSpeed * Time.deltaTime, raccoon.transform.position.y, 0);
            yield return null;
        }
    }

    public IEnumerator MoveMoneyBags()
    {
        yield return new WaitForSeconds(10f);
        for (float i = 0f; i > -24f; i -= streetMoveSpeed * Time.deltaTime)
        {
            moneyBags.transform.position = new Vector3(moneyBags.transform.position.x - streetMoveSpeed * Time.deltaTime, moneyBags.transform.position.y, 0);
            yield return null;
        }
    }

    public IEnumerator MoveMachine()
    {
        yield return new WaitForSeconds(7f);
        for (float i = 0f; i > -24f; i -= streetMoveSpeed * Time.deltaTime)
        {
            machine.transform.position = new Vector3(machine.transform.position.x - streetMoveSpeed * Time.deltaTime, machine.transform.position.y, 0);
            yield return null;
        }
    }

    public IEnumerator MoveBall()
    {
        yield return new WaitForSeconds(22f);
        for (float i = 0f; i > -15f; i -= streetMoveSpeed * Time.deltaTime)
        {
            ball.transform.position = new Vector3(ball.transform.position.x - streetMoveSpeed * Time.deltaTime, ball.transform.position.y, 0);
            yield return null;
        }
        StartCoroutine("Jump");
    }

    public IEnumerator Jump()
    {
        ball.GetComponent<SpriteRenderer>().enabled = false;

        player.transform.Find("Ball").gameObject.SetActive(true);

        for (float i = 0f; i < 10f; i += 3f * Time.deltaTime)
        {
            streetMoveSpeed += i / 20;
            buildingMoveSpeed += i / 20;
            skyMoveSpeed += i / 20;
            player.transform.Find("Ball").gameObject.GetComponent<Animator>().speed = i;
            yield return null;
        }
    }

    public IEnumerator JumpFar()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 1f;
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(4, 1) * 1000);
        this.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1f);

        for (float i = black.GetComponent<SpriteRenderer>().color.a; i <= 1f; i += 0.3f * Time.deltaTime)
        {
            Color c = black.GetComponent<SpriteRenderer>().color;
            c.a = i;
            black.GetComponent<SpriteRenderer>().color = c;
            yield return null;
        }
        over = true;
    }
}
