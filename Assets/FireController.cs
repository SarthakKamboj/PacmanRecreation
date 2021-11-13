using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoxColliderSettings
{
    public Vector2 offset;
    public Vector2 size;
}

public class FireController : MonoBehaviour
{


    [Header("Box collider settings")]
    public BoxColliderSettings fireOffSettings;
    public BoxColliderSettings fireOnSettings;

    public float TimeBetweenFireSwitch = 3f;

    float timeSinceLastFireSwitch;
    [HideInInspector] public bool fireOn = false;

    BoxCollider2D boxCollider;
    Animator fireAnimator;

    void Start()
    {
        timeSinceLastFireSwitch = TimeBetweenFireSwitch;
        boxCollider = GetComponent<BoxCollider2D>();
        fireAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        timeSinceLastFireSwitch = Mathf.Max(0f, timeSinceLastFireSwitch - Time.deltaTime);

        if (timeSinceLastFireSwitch == 0f)
        {
            timeSinceLastFireSwitch = TimeBetweenFireSwitch;
            fireOn = !fireOn;
            fireAnimator.SetBool("fireOn", fireOn);
            boxCollider.offset = fireOn ? fireOnSettings.offset : fireOffSettings.offset;
            boxCollider.size = fireOn ? fireOnSettings.size : fireOffSettings.size;
        }
    }
}
