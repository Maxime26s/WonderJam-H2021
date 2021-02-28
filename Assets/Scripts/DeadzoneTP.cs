using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneTP : MonoBehaviour
{
    public GameObject zap;
    private void OnTriggerExit2D(Collider2D collision)
    {
        zap.GetComponent<zap>().leaving(gameObject);
    }
}
