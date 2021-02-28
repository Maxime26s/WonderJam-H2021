using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utilMenu : MonoBehaviour
{


    public void ExitGame()
    {
        Application.Quit();
    }

    public void hideObject()
    {
        GameObject go = gameObject;
        go.SetActive(false);
    }

    public void showObject()
    {
        GameObject go = gameObject;
        go.SetActive(true);
    }

}
