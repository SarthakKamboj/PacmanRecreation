using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    void Start()
    {
        GameObject[] backgroundMusics = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if (backgroundMusics.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

}
