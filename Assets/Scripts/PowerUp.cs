using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string type;
    public bool throwable;
    public bool used = false;
    private float zoomSpeed = 0.24f;
    public float timeout;
    public Vector2 startingPos, startingSize;

    private void Start()
    {
        startingPos = transform.localPosition;
        startingSize = transform.localScale;
    }

    private void Update()
    {
        if (!used)
        {
            transform.Rotate(new Vector3(0, 0, 15) * Time.deltaTime);
            transform.localScale += Vector3.one * zoomSpeed * Time.deltaTime;
            if (transform.localScale.x >= 1.5f || transform.localScale.x <= 0.66f)
                zoomSpeed = zoomSpeed * -1;
        }
    }

    public void OnCollection()
    {
        transform.localScale = new Vector2(1.5f, 1.5f);
        gameObject.SetActive(false);
        //Ajouter au UI
    }

    public void SetUsed()
    {
        used = true;
    }
}
