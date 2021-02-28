using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

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
    public int world = 1, level = -1;
    public string levelName = "";
    public TextMeshProUGUI title;
    public List<GameObject> panels;
    public State gameState;
    public GameObject[] players;
    public GameObject timer;

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

    public Animator timerAnimator, showcaseAnimator, summaryAnimator;
    public GameObject summary;
    public List<GameObject> collectedP1, collectedP2;
    public List<GameObject> itemsP1, itemsP2;
    public TextMeshProUGUI scorep1, scorep2, winner;
    public GameObject cameraShowcase, cameraSummary;

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

    public void Next()
    {
        switch (gameState)
        {
            case State.Showcase:
                foreach (GameObject player in players)
                    player.GetComponent<PlayerController>().isFrozen = false;
                gameState = State.Playing;
                timer.GetComponent<Timer>().running = true;
                timer.SetActive(true);
                cameraShowcase.SetActive(false);
                break;
            case State.Summary:
                GoNext();
                break;
            case State.Playing:
            default:
                break;
        }
    }

    public void TimeOut()
    {
        timer.GetComponent<Timer>().running = false;
        foreach (GameObject player in players)
            player.GetComponent<PlayerController>().isFrozen = true;
        gameState = State.Summary;


        scorep1.text = players[0].GetComponent<ColObjectives>().cash.ToString() + " $";
        scorep2.text = players[1].GetComponent<ColObjectives>().cash.ToString() + " $";

        if (players[0].GetComponent<ColObjectives>().cash < players[1].GetComponent<ColObjectives>().cash)
            winner.text = "Player 2 remporte la manche!";
        else
            winner.text = "Player 1 remporte la manche!";

        for (int i = 0; i < collectedP1.Count; i++)
        {
            itemsP1[i].GetComponent<Image>().sprite = collectedP1[i].GetComponent<SpriteRenderer>().sprite;
        }
        for (int i = 0; i < collectedP2.Count; i++)
        {
            itemsP2[i].GetComponent<Image>().sprite = collectedP2[i].GetComponent<SpriteRenderer>().sprite;
        }
        cameraSummary.SetActive(true);
        summary.SetActive(true);
        timerAnimator.SetTrigger("Start");

        IEnumerator ShowItem(float temps, int i, PlayerEnum player)
        {
            yield return new WaitForSeconds(temps * i);
            switch (player)
            {
                case PlayerEnum.One:
                    itemsP1[i].SetActive(true);
                    break;
                case PlayerEnum.Two:
                    itemsP2[i].SetActive(true);
                    break;
                default:
                    break;
            }
        }

        IEnumerator WaitBeforeShow()
        {
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i < collectedP1.Count; i++)
            {
                StartCoroutine(ShowItem(0.33f, i, PlayerEnum.One));
            }
            for (int i = 0; i < collectedP2.Count; i++)
            {
                StartCoroutine(ShowItem(0.33f, i, PlayerEnum.One));
            }
        }

        StartCoroutine(WaitBeforeShow());

    }

    private void OnEnable()
    {
        try
        {
            policeEffect1 = GameObject.Find("P2").transform.GetChild(1).gameObject;
            policeEffect2 = GameObject.Find("P2").transform.GetChild(0).gameObject;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        players = GameObject.FindGameObjectsWithTag("Player");
        players[0].transform.position = spawn1.transform.position;
        players[1].transform.position = spawn2.transform.position;

        title.text = world + " - " + level + "\n" + levelName;

        for (int i = 0; i < 6; i++)
        {
            if (loot[i] != null)
            {
                Objectif obj = loot[i].GetComponent<Objectif>();
                panels[i].GetComponentInChildren<TextMeshPro>().text = obj.name + "\n" + obj.money + " $";
                panels[i].GetComponentInChildren<Image>().sprite = loot[i].GetComponent<SpriteRenderer>().sprite;
            }
            else
                panels[i].SetActive(false);
        }
    }

    public void UpdateUI(GameObject player)
    {
        if (player.GetComponent<PlayerController>().playerNum == PlayerEnum.One)
        {
            textP1.text = player.GetComponent<ColObjectives>().cash.ToString() + "$";
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
            textP2.text = player.GetComponent<ColObjectives>().cash.ToString() + "$";
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
    public enum State
    {
        Showcase,
        Summary,
        Playing
    }
}