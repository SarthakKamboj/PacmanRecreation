using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFruit : MonoBehaviour
{

    public LayerMask characterLayerMask;

    FruitManager fruitManager;

    private void Start()
    {
        fruitManager = GameObject.FindObjectOfType<FruitManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayerMaskUtils.IsInLayerMask(collider.gameObject, characterLayerMask))
        {
            fruitManager.FruitDeleted();
            Destroy(gameObject);
        }
    }
}
