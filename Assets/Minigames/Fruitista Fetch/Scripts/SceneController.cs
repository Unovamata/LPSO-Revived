using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static private bool IsSceneLoaded = false;
    void Start()
    {
        if (IsSceneLoaded == false)
        {
            SceneManager.LoadScene(1);
            IsSceneLoaded = true;
            Debug.Log("SceneLoadedTrue");

        }
        else
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
