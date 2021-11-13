using UnityEngine;

public class FireMakeCharacterDie : MonoBehaviour
{

    public LayerMask characterLayerMask;
    FireController fireController;

    private void Start()
    {
        fireController = GetComponent<FireController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskUtils.IsInLayerMask(collision.collider.gameObject, characterLayerMask) && fireController.fireOn)
        {
            collision.collider.GetComponent<CharacterDie>().Die();
        }
    }
}
