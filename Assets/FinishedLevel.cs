using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedLevel : MonoBehaviour
{

    public Animator endpointAnimator;
    public LayerMask characterLayerMask;

    [HideInInspector] public bool finishedLevel = false;

    // void OnCollisionEnter2D(Collision2D collision)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayerMaskUtils.IsInLayerMask(collider.gameObject, characterLayerMask) && finishedLevel)
        {
            endpointAnimator.SetBool("finished", true);
        }
    }
}
