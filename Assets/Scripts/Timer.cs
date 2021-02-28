using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public float minutes = 1;
    public float seconds = 0;
    public float miliseconds = 0;
    public bool running = false;
    public float secYellow = 30, secRed = 10;

    private void Start()
    {
        if (timer == null)
            timer = GetComponentInChildren<TextMeshProUGUI>();
        RefreshText();
    }

    void Update()
    {
        if (running)
        {
            if (miliseconds <= 0)
            {
                if (seconds <= 0)
                {
                    minutes--;
                    seconds = 59;
                }
                else if (seconds >= 0)
                {
                    seconds--;
                }

                miliseconds = 100;
            }

            miliseconds -= Time.deltaTime * 100;
            DeathCheck();
        }
    }

    public void RefreshText()
    {
        if (minutes > 0)
            timer.text = string.Format("{0:#0}:{1:00}", minutes, seconds);
        else
        {
            if (seconds < secRed)
                timer.color = new Color32(255, 54, 74, 255);
            else if (seconds < secYellow)
                timer.color = new Color32(240, 139, 31, 255);
            timer.text = string.Format("{0:#0}:{1:00}", seconds, (int)miliseconds);
        }
    }

    void DeathCheck()
    {
        if (minutes < 0)
        {
            minutes = 0;
            seconds = 0;
            miliseconds = 0;
            running = false;
            GameManager.Instance.GoNext();
        }
        RefreshText();
    }
}