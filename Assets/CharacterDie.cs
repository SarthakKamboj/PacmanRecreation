using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDie : MonoBehaviour
{

    public Vector2 dieUpForce;
    public float playerYPos = -20f;
    public SpriteRenderer characterRenderer;
    public Animator characterAnimator;

    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    CharacterMove characterMove;
    AudioSource dieAudioSource;

    bool died = false;
    float timeSinceDied = 0f;
    Vector3 origRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        characterMove = GetComponent<CharacterMove>();
        dieAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (died)
        {
            Vector3 newRotation = Vector3.Slerp(origRotation, new Vector3(0f, 0f, (characterRenderer.flipX ? -1 : 1) * 60f), timeSinceDied);

            Quaternion quat = transform.rotation;
            quat.eulerAngles = newRotation;
            transform.rotation = quat;

            timeSinceDied += Time.deltaTime * 5f;
            characterAnimator.SetBool("dead", true);

            if (transform.position.y < playerYPos)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void Die()
    {

        dieAudioSource.Play();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        rb.simulated = false;
        boxCollider.enabled = false;
        died = true;
        origRotation = transform.rotation.eulerAngles;
        characterMove.enabled = false;

        Invoke("AddDieForce", 0.75f);
    }

    public void AddDieForce()
    {
        rb.simulated = true;
        rb.AddForce(dieUpForce, ForceMode2D.Impulse);
    }
}
