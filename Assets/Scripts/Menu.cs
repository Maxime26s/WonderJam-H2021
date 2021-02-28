using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject background;
    public GameObject backgroundSprite;
    public float xSpeed;
    public float ySpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MoveBackground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveBackground()
    {
        for (float i = 0f; i < 2.8f; i += 1f * Time.deltaTime)
        {
            background.transform.position = new Vector3(background.transform.position.x - xSpeed * Time.deltaTime, background.transform.position.y + ySpeed * Time.deltaTime, background.transform.position.z);
            yield return null;
        }

        for (float i = 0f; i < 2.8f; i += 1f * Time.deltaTime)
        {
            background.transform.position = new Vector3(background.transform.position.x + xSpeed * Time.deltaTime, background.transform.position.y - ySpeed * Time.deltaTime, background.transform.position.z);
            yield return null;
        }
        StartCoroutine("MoveBackground");
    }
}
