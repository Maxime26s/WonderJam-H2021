using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class updateImage : MonoBehaviour
{

    public Image image;

    private Sprite newImage;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (image.name == "containerPowerUpP1")
            newImage = //getpowerupP1;
        else if(image.name == "containerPowerUpP2")
            newImage = //getPowerupP2;
        else if (image.name == "containerObjetTransporteP1")
            newImage =  //getObjTranspoP1;
        else //containerObjetTransporteP2
            newImage = //getObjTranspoP2;

                image.image = newImage;*/
    }
    
    
}
