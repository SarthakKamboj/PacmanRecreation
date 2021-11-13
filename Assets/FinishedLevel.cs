using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedLevel : MonoBehaviour
{

    public Animator endpointAnimator;
    public LayerMask characterLayerMask;

    [HideInInspector] public bool finishedLevel = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayerMaskUtils.IsInLayerMask(collider.gameObject, characterLayerMask) && finishedLevel)
        {
            endpointAnimator.SetBool("finished", true);
            Invoke("LoadNextLevel", 2f);
            collider.GetComponent<CharacterMove>().enabled = false;
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
