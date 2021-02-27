using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScriptSceneThingy : MonoBehaviour
{
    public LevelLoader loader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            loader.LoadNextLevelAdditive(3);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            loader.LoadNextLevelAdditive(4);
        }
    }
}
