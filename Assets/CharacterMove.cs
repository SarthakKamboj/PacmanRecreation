using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{

    public Rigidbody2D rb;
    public LayerMask wallLayerMask;
    public Collider2D pacmanCollider;
    public float TimeBetweenClimbs = 0.5f;
    public float TimeBetweenJumps = 0.5f;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    bool grounded = false, climbing = false;

    float timeSinceLastClimb = 0f;
    float timeSinceLastJump = 0f;

    float numJumpsSinceLastGrounded = 0;

    ContactFilter2D contactFilter;

    void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(wallLayerMask);
    }

    void Update()
    {
        timeSinceLastClimb = Mathf.Max(0f, timeSinceLastClimb - Time.deltaTime);
        timeSinceLastJump = Mathf.Max(0f, timeSinceLastJump - Time.deltaTime);

        bool hittingWallLeft = IsHittingWallLeft();
        bool hittingWallRight = isHittingWallRight();

        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        animator.SetFloat("horizontalVelocity", Mathf.Abs(hor));
        animator.SetFloat("verticalVelocity", rb.velocity.y);

        if (hor < 0f && !hittingWallLeft)
        {
            transform.position = transform.position + (Vector3.left * 3f * Time.deltaTime);
            spriteRenderer.flipX = true;
        }
        else if (hor > 0f && !hittingWallRight)
        {
            transform.position = transform.position + (Vector3.right * 3f * Time.deltaTime);
            spriteRenderer.flipX = false;
        }

        float thres = 0.1f;

        bool shouldJump = Input.GetKeyDown(KeyCode.Space) && timeSinceLastJump <= 0;
        bool prevClimbing = climbing;
        // climbing = (hittingWallLeft || hittingWallRight) && Input.GetKey(KeyCode.LeftShift) && timeSinceLastClimb == 0f;
        climbing = false;

        if (prevClimbing && !climbing)
        {
            timeSinceLastClimb = TimeBetweenClimbs;
        }

        animator.SetBool("isClimbing", climbing);

        if (climbing)
        {
            bool climbingActive = Mathf.Abs(ver) >= thres;

            if (climbingActive)
            {
                transform.position = transform.position + (Vector3.up * Mathf.Sign(ver) * 3f * Time.deltaTime);
            }

            animator.SetBool("isClimbingActive", climbingActive);

            rb.gravityScale = 0f;

            Vector2 vel = rb.velocity;
            vel.y = 0f;
            rb.velocity = vel;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        if (shouldJump)
        {
            numJumpsSinceLastGrounded += 1;
            animator.SetFloat("numJumpsSinceLastGrounded", numJumpsSinceLastGrounded);

            if (climbing)
            {
                timeSinceLastClimb = TimeBetweenClimbs;
            }

            timeSinceLastJump = TimeBetweenJumps;
            rb.AddForce(new Vector2(0f, 5.5f), ForceMode2D.Impulse);
        }

    }

    bool DetectedFromRaycast(Vector3[] positions, Vector2 direction, ContactFilter2D contactFilter, float raycastDistance)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];

        for (int i = 0; i < positions.Length; i++)
        {
            int numColliders = Physics2D.Raycast(positions[i], direction, contactFilter, hits, raycastDistance);
            if (numColliders > 0)
            {
                return true;
            }
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D[] groundHits = new RaycastHit2D[1];

        float xExtent = pacmanCollider.bounds.extents.x;
        float yExtent = pacmanCollider.bounds.extents.y;
        const float raycastDistance = 0.1f;

        Vector3[] positions = {
            new Vector3(-xExtent, -yExtent, 0f) ,
            new Vector3(-xExtent / 2, -yExtent, 0f) ,
            new Vector3(0f, -yExtent, 0f) ,
            new Vector3(xExtent / 2, -yExtent, 0f) ,
            new Vector3(xExtent, -yExtent, 0f) ,
        };

        Vector3 transformPos = transform.position;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += transformPos;
        }

        if (DetectedFromRaycast(positions, Vector2.down, contactFilter, raycastDistance))
        {
            grounded = true;
            numJumpsSinceLastGrounded = 0;
            animator.SetFloat("numJumpsSinceLastGrounded", numJumpsSinceLastGrounded);
        }
    }

    bool IsHittingWallLeft()
    {
        float xExtent = pacmanCollider.bounds.extents.x;
        float yExtent = pacmanCollider.bounds.extents.y;

        const float raycastDistance = 0.025f;

        Vector3[] leftWallRaycastPositions =
        {
            new Vector3(-xExtent, yExtent, 0f),
            new Vector3(-xExtent, yExtent / 2f, 0f),
            new Vector3(-xExtent, 0f, 0f),
            new Vector3(-xExtent, -yExtent / 2f, 0f),
            new Vector3(-xExtent, -yExtent, 0f)
        };

        Vector3 transformPos = transform.position;
        for (int i = 0; i < leftWallRaycastPositions.Length; i++)
        {
            leftWallRaycastPositions[i] += transformPos;
        }

        return DetectedFromRaycast(leftWallRaycastPositions, Vector2.left, contactFilter, raycastDistance);
    }

    bool isHittingWallRight()
    {
        float xExtent = pacmanCollider.bounds.extents.x;
        float yExtent = pacmanCollider.bounds.extents.y;

        const float raycastDistance = 0.025f;

        Vector3[] rightWallRaycastPositions =
        {
            new Vector3(xExtent, yExtent, 0f),
            new Vector3(xExtent, yExtent / 2f, 0f),
            new Vector3(xExtent, 0f, 0f),
            new Vector3(xExtent, -yExtent / 2f, 0f),
            new Vector3(xExtent, -yExtent, 0f)
        };

        Vector3 transformPos = transform.position;
        for (int i = 0; i < rightWallRaycastPositions.Length; i++)
        {
            rightWallRaycastPositions[i] += transformPos;
        }

        return DetectedFromRaycast(rightWallRaycastPositions, Vector2.right, contactFilter, raycastDistance);
    }
}
