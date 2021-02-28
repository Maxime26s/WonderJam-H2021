using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColObjectives : MonoBehaviour
{
    public float cash = 0;
    public bool holdingObjective = false;
    public GameObject objective;
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
            }
        }
        else if (collision.gameObject.CompareTag("Zone"))
        {
            if (holdingObjective)
            {
                objective.SetActive(true);
                holdingObjective = false;
                cash += objective.GetComponent<Objectif>().money;
                //Enlever du UI                Debug.Log(cash);
                GameManager.Instance.collected++;
                GameManager.Instance.collectedP1.Add(objective);
                Destroy(objective);
                objective = null;
            }

        }
        GameManager.Instance.UpdateUI(gameObject);
    }
}
