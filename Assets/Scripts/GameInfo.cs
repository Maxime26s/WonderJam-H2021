using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public LobbyScript lobbyScript;
    public bool started = false;
    
    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        lobbyScript.Login(players.Count);
    }

    private void Start()
    {
        lobbyScript = GameObject.Find("LobbyScript").GetComponent<LobbyScript>();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("PlayerInfo"));
        Debug.Log("Ready2");
    }
}
