using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public List<GameObject> players = new List<GameObject>();
    public GameObject texteP1, texteP2, p1guy, p2guy;
    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        switch (players.Count)
        {
            case 1:
                Destroy(texteP1);
                p1guy.SetActive(true);
                break;
            case 2:
                Destroy(texteP2);
                p2guy.SetActive(true);
                break;
            default:
                Debug.Log("FAI LWATHFASDFDSA");
                break;
        }
    }

}
