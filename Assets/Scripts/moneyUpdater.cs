using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class moneyUpdater : MonoBehaviour
{

    private int money;
    public TextMeshProUGUI textMoney;
    // Start is called before the first frame update
    void Start()
    {
        textMoney = gameObject.GetComponent<TextMeshProUGUI>();
        money = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textMoney.SetText(money + "$");
        /*if (textMoney.name == "textMoneyP1")
        {
            money = //getmoneyP1
        }
        else //textMoneyP2
        {
            money = //getmoneyP2
        }*/
    }
}
