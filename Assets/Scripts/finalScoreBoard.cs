using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class finalScoreBoard : MonoBehaviour
{


    public ColObjectives P1, P2;
    public TextMeshProUGUI winner, scoreP1, scoreP2;
    // Start is called before the first frame update
    void Start()
    {
            if(P1.cash > P2.cash)
                winner.SetText("Gagnant : Joueur 1");
            else if(P1.cash < P2.cash)
                winner.SetText("Gagnant : Joueur 2");
            else
                winner.SetText("Ex aequo !");
        
            scoreP1.SetText("Stash P1 : " + P1.cash);
            scoreP2.SetText("Stash P2 : " + P2.cash);
    }

}
