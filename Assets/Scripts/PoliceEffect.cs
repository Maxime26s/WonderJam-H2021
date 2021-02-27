using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PoliceEffect : MonoBehaviour
{
    public Light2D light2D;
    public Color blue, red;
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        light2D.color = Color.Lerp(blue, red, Mathf.PingPong(Time.time, 1));
    }
}
