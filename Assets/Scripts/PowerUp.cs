using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string type;
    public bool throwable;

    private void Start()
    {
    }

    public void OnCollection()
    {
        gameObject.SetActive(false);
        //Ajouter au UI
    }
}
