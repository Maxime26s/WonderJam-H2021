using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
    public GameObject texteP1, texteP2, p1guy, p2guy;
    public LevelLoader loader;
    public GameInfo gameInfo;

    public void Login(int count)
    {
        switch (count)
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

    public void StartGame()
    {
        gameInfo.CheckCams();
        loader.LoadNextLevelAdditive(3);
    }
}
