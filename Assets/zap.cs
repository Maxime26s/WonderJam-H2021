using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zap : MonoBehaviour
{
    public GameObject zap1, zap2;
    private bool left1;
    private bool left2;
    public void tp(Collider2D collision, GameObject zap)
    {
        if(zap == zap1 && left1)
        {
            collision.gameObject.transform.position = new Vector2(zap2.transform.position.x, zap2.transform.position.y);
            left2 = false;
        }
        else if (zap == zap2 && left2)
        {
            collision.gameObject.transform.position = new Vector2(zap1.transform.position.x, zap1.transform.position.y);
            left1 = false;
        }
        

    }
    public void leaving(GameObject zap)
    {
        if (zap == zap1)
        {
            left1 = true;
        }
        else if (zap == zap2)
        {
            left2 = true;
        }
    }
}
