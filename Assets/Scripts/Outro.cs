using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outro : MonoBehaviour
{
    public GameObject street;
    public GameObject building;
    public GameObject sky;
    public GameObject player;
    public GameObject text;
    public GameObject[] cars;
    public GameObject[] balls;

    public float streetMoveSpeed;
    public float buildingMoveSpeed;
    public float skyMoveSpeed;
    public float textMoveSpeed;

    private Vector3 initialStreet;
    private Vector3 initialBuilding;
    private Vector3 initialSky;
    public Vector3[] initialCars;
    private bool moving;
    private bool over = false, done = false;

    // Start is called before the first frame update
    void Start()
    {
        player = balls[0];
        balls[0].SetActive(true);

        for (int i = 0; i < cars.Length; i++)
            initialCars[i] = cars[i].transform.position;

        initialStreet = street.transform.position;
        initialBuilding = building.transform.position;
        initialSky = sky.transform.position;

        moving = true;

        StartCoroutine("MoveText");
        StartCoroutine("MoveStreet");
        StartCoroutine("MoveBuildings");
        StartCoroutine("MoveSky");
        StartCoroutine("MoveCars");
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<Animator>().SetBool("Running", true);

        if (over && !done)
        {
            done = true;
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadMenu();
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
        for (float i = -550f; i < 0f; i += textMoveSpeed * Time.deltaTime)
        {
            text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, text.GetComponent<RectTransform>().anchoredPosition.y + textMoveSpeed * Time.deltaTime, 0);
            yield return null;
        }
        over = true;
    }

    public IEnumerator MoveCars()
    {
        for(int i = 0; i < cars.Length; i++)
        {
            cars[i].transform.position = new Vector3(cars[i].transform.position.x, Mathf.Clamp(Random.Range(cars[i].transform.position.y - 0.1f, cars[i].transform.position.y + 0.1f), initialCars[i].y - 0.3f, initialCars[i].y + 0.3f), 0);
        }
        yield return new WaitForSeconds(0.1f);

       StartCoroutine("MoveCars");
    }
}
