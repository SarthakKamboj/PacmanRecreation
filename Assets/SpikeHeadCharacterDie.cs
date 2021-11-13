using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHeadCharacterDie : MonoBehaviour
{

    public LayerMask characterLayerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskUtils.IsInLayerMask(collision.collider.gameObject, characterLayerMask))
        {
            collision.collider.GetComponent<CharacterDie>().Die();
        }
    }
}
