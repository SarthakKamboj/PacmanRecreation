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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskUtils.IsInLayerMask(collision.gameObject, characterLayerMask))
        {
            fruitManager.FruitDeleted();
            Destroy(gameObject);
        }
    }
}
