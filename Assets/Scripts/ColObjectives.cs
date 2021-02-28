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
                switch (GetComponent<PlayerController>().playerNum)
                {
                    case PlayerEnum.One:
                        GameManager.Instance.collectedP1.Add(objective.GetComponentInChildren<SpriteRenderer>().sprite);
                        break;
                    case PlayerEnum.Two:
                        GameManager.Instance.collectedP2.Add(objective.GetComponentInChildren<SpriteRenderer>().sprite);
                        break;
                    default:
                        break;
                }
                Destroy(objective);
                objective = null;
            }
        }
    }
}
