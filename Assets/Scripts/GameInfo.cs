using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using Cinemachine;

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
    public CinemachineVirtualCamera vcam1, vcam2;
    public Camera cam1, cam2;
    public GameObject barreNoire;

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        lobbyScript.Login(players.Count);
        switch (players.Count)
        {
            case 1:
                player.GetComponent<PlayerController>().playerNum = PlayerEnum.One;
                vcam1.Follow = player.transform;
                cam1.gameObject.SetActive(false);
                cam1.gameObject.SetActive(true);
                break;
            case 2:
                player.GetComponent<PlayerController>().playerNum = PlayerEnum.Two;
                vcam2.Follow = player.transform;
                cam2.gameObject.SetActive(false);
                cam2.gameObject.SetActive(true);
                break;

        }
    }

    public void AddConfiner(Collider2D confiner)
    {
        CinemachineConfiner cineConfiner1 = vcam1.GetComponent<CinemachineConfiner>(), cineConfiner2 = vcam2.GetComponent<CinemachineConfiner>();
        cineConfiner1.m_BoundingShape2D = confiner;
        cineConfiner2.m_BoundingShape2D = confiner;
    }

    private void Start()
    {
        lobbyScript = GameObject.Find("LobbyScript").GetComponent<LobbyScript>();
        lobbyScript.gameInfo = this;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("PlayerInfo"));
    }

    public void CheckCams()
    {
        /*
        if (players.Count != 2)
        {
            cam1.rect = new Rect(0, 0, 0, 0);
            cam2.gameObject.GetComponent<UniversalAdditionalCameraData>().cameraStack.Remove(overlaycam);
            Destroy(cam2.gameObject);
            Destroy(vcam2.gameObject);
        }*/
    }
}
