using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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

    public LevelLoader loader;
    public GameObject spawn1, spawn2;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> loot = new List<GameObject>();
    public int collected;
    public bool isSuperAlert = false;
    public GameObject policeEffect1, policeEffect2;
    public GameObject cam1, cam2;

    [Header("UI")]
    public TextMeshProUGUI textP1;
    public TextMeshProUGUI textP2;
    public Image collP1;
    public Image collP2;
    public Image PUP1;
    public Image PUP2;
    public Image cashP1;
    public Image cashP2;
    public Sprite[] cashIcons;
    public Sprite transparent;

    public void SuperAlert(Transform transform)
    {
        IEnumerator SuperDuperAlertTimer()
        {
            isSuperAlert = true;
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyAI>().target = transform;
            }
            policeEffect1.SetActive(true);
            policeEffect2.SetActive(true);
            yield return new WaitForSeconds(8f);
            isSuperAlert = false;
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyAI>().target = enemy.GetComponent<EnemyAI>().route[enemy.GetComponent<EnemyAI>().routeIndex];
            }
            policeEffect1.SetActive(false);
            policeEffect2.SetActive(false);
        }
        StartCoroutine(SuperDuperAlertTimer());
    }

    private void Update()
    {
        if (collected >= loot.Count)
            GoNext();
    }

    public void GoNext()
    {
        loader.LoadNextLevelAdditive(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnEnable()
    {
        try
        {
            policeEffect1 = GameObject.Find("P2").transform.GetChild(1).gameObject;
            policeEffect2 = GameObject.Find("P2").transform.GetChild(0).gameObject;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        Setup();
    }

    public void Setup()
    {
        GameObject[] cams = GameObject.FindGameObjectsWithTag("Camera");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
        if (players.Length > 0)
        {
            spawn1 = spawns[0];
            players[0].transform.position = spawn1.transform.position;
            cams[0].GetComponent<CinemachineVirtualCamera>().Follow = players[0].transform;
        }
        if (players.Length > 1)
        {
            spawn1 = spawns[1];
            players[1].transform.position = spawn2.transform.position;
            cams[1].GetComponent<CinemachineVirtualCamera>().Follow = players[1].transform;
        }


    }

    public void UpdateUI(GameObject player)
    {
        if(player.GetComponent<PlayerController>().playerNum == PlayerEnum.One)
        {
            textP1.text = String.Format("{0:n0}$", player.GetComponent<ColObjectives>().cash).Replace(",", " ");
            if (player.GetComponent<ColObjectives>().holdingObjective)
            {
                collP1.sprite = player.GetComponent<ColObjectives>().objective.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                collP1.sprite = transparent;
            }
            if (player.GetComponent<PlayerController>().holding)
            {
                PUP1.sprite = player.GetComponent<PlayerController>().objectHolding.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                PUP1.sprite = transparent;
            }
            float m = player.GetComponent<ColObjectives>().cash;
            if (m <= 75000)
            {
                cashP1.sprite = cashIcons[0];
            }
            if (m > 75000)
            {
                cashP1.sprite = cashIcons[1];
            }
            if (m > 125000)
            {
                cashP1.sprite = cashIcons[2];
            }
            if (m > 200000)
            {
                cashP1.sprite = cashIcons[3];
            }
        }
        if (player.GetComponent<PlayerController>().playerNum == PlayerEnum.Two)
        {
            textP2.text = String.Format("{0:n0}$", player.GetComponent<ColObjectives>().cash).Replace(",", " ");
            if (player.GetComponent<ColObjectives>().holdingObjective)
            {
                collP2.sprite = player.GetComponent<ColObjectives>().objective.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                collP2.sprite = transparent;
            }
            if (player.GetComponent<PlayerController>().holding)
            {
                PUP2.sprite = player.GetComponent<PlayerController>().objectHolding.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                PUP2.sprite = transparent;
            }
            float m = player.GetComponent<ColObjectives>().cash;
            if (m <= 75000)
            {
                cashP2.sprite = cashIcons[0];
            }
            if (m > 75000)
            {
                cashP2.sprite = cashIcons[1];
            }
            if (m > 125000)
            {
                cashP2.sprite = cashIcons[2];
            }
            if (m > 200000)
            {
                cashP2.sprite = cashIcons[3];
            }
        }
    }
}