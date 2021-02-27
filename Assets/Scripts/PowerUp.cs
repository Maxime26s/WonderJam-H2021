using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string type;
    public bool throwable;
    public bool used = false;
    private float zoomSpeed = 0.24f;

    private void Update()
    {
        if (!used)
        {
            transform.Rotate(new Vector3(0,0,15) * Time.deltaTime);
            transform.localScale += Vector3.one * zoomSpeed * Time.deltaTime;
            if (transform.localScale.x >= 1.5f || transform.localScale.x <= 0.66f)
                zoomSpeed = zoomSpeed * -1;
        }
    }

    public void OnCollection()
    {
        gameObject.SetActive(false);
        //Ajouter au UI
    }

    public void SetUsed()
    {
        used = true;
    }
}
