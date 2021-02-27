using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    //Animator animator;
    bool split = false;
    public CinemachineVirtualCamera vcam1, vcam2, vcamGroup;
    public GameObject groupCam;
    void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwitchPriority();
        }
    }

    void SwitchPriority()
    {
        if (split)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 0;
            vcamGroup.Priority = 1;
           // vcamGroup.gameObject.SetActive(true);
            //groupCam.SetActive(true);
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 1;
            vcamGroup.Priority = 0;
           // vcamGroup.gameObject.SetActive(false);
            //groupCam.SetActive(false);
        }
        split = !split;
    }
}
