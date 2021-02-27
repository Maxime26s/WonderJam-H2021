using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColObjectives : MonoBehaviour
{
    bool holdingObjective = false;
    GameObject objective;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Objectif"))
        {
            if (!holdingObjective)
            {
                holdingObjective = true;
                objective = collision.gameObject;
                objective.SetActive(false);
                //Ajouter au UI
                Debug.Log("Objectif récupéré");
            }
        }
        else if (collision.gameObject.CompareTag("Zone"))
        {
            if (holdingObjective)
            {
                Destroy(objective);
                objective = null;
                holdingObjective = false;
                //Ajouter argent ou whatever
                //Enlever du UI
                Debug.Log("Objectif déposé");
            }
            
        }
    }
}
