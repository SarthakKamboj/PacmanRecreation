using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{

    public LayerMask fruitLayerMask;
    public int totalNumFruits;
    public FinishedLevel finishedLevel;
    public AudioSource audioSource;

    void Start()
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (LayerMaskUtils.IsInLayerMask(gameObjects[i], fruitLayerMask))
            {
                totalNumFruits += 1;
            }
        }
    }

    public void FruitDeleted()
    {
        if (totalNumFruits == 0) return;

        totalNumFruits -= 1;
        audioSource.Play();

        if (totalNumFruits == 0)
        {
            finishedLevel.finishedLevel = true;
        }
    }
}
