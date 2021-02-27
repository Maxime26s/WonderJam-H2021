using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniZap : MonoBehaviour
{
    public zap papa;
    void OnTriggerEnter2D(Collider2D collision)
    { 
        papa.tp(collision, gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        papa.leaving(gameObject);
    }

}
