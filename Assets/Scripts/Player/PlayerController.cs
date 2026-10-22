using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float laneDistance = 1.6f;
    [SerializeField] private float laneChangeSpeed = 12f;

    [Header("Zıplama")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Kayma")]
    [SerializeField] private float slideDuration = 1.3f;

    [SerializeField, Range(0.1f, 1f)]
    private float slideColliderHeightMultiplier = 0.5f;

    [Header("Bağlantılar")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioManager audioManager;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    // 0 = Sol, 1 = Orta, 2 = Sağ
    private int desiredLane = 1;
    private float startingX;

    private bool jumpRequested;
    private bool isSliding;
    private bool startAnimationSet;

    private Vector3 normalColliderSize;
    private Vector3 normalColliderCenter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        rb.useGravity = true;
        rb.isKinematic = false;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        startingX = rb.position.x;

        normalColliderSize = boxCollider.size;
        normalColliderCenter = boxCollider.center;
    }

    private void Start()
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;

        if (maxSpeed < forwardSpeed)
            maxSpeed = forwardSpeed;
    }

    private void Update()
    {
        if (!PlayerManager.isGameStarted ||
            PlayerManager.gameOver)
        {
            return;
        }

        if (!startAnimationSet)
        {
            startAnimationSet = true;

            if (animator != null)
            {
                animator.SetBool(
                    "isGameStarted",
                    true
                );
            }
        }

        forwardSpeed = Mathf.MoveTowards(
            forwardSpeed,
            maxSpeed,
            acceleration * Time.deltaTime
        );

        HandleSwipeInput();
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted ||
            PlayerManager.gameOver)
        {
            return;
        }

        MovePlayer();
        HandleJump();
    }

    private void HandleSwipeInput()
    {
        if (SwipeManager.swipeRight)
        {
            desiredLane = Mathf.Clamp(
                desiredLane + 1,
                0,
                2
            );
        }

        if (SwipeManager.swipeLeft)
        {
            desiredLane = Mathf.Clamp(
                desiredLane - 1,
                0,
                2
            );
        }

        if (SwipeManager.swipeUp &&
            !isSliding &&
            IsGrounded())
        {
            jumpRequested = true;
        }

        if (SwipeManager.swipeDown &&
            !isSliding &&
            IsGrounded())
        {
            StartCoroutine(Slide());
        }
    }

    private void MovePlayer()
    {
        float targetX =
            startingX +
            (desiredLane - 1) * laneDistance;

        float nextX = Mathf.MoveTowards(
            rb.position.x,
            targetX,
            laneChangeSpeed * Time.fixedDeltaTime
        );

        Vector3 velocity = rb.linearVelocity;

        velocity.x =
            (nextX - rb.position.x) /
            Time.fixedDeltaTime;

        velocity.z = forwardSpeed;

        rb.linearVelocity = velocity;
    }

    private void HandleJump()
    {
        if (!jumpRequested)
            return;

        jumpRequested = false;

        /*
         * Update ile FixedUpdate arasında oyuncu
         * yerden ayrılmış olabilir. Bu yüzden
         * burada tekrar kontrol ediyoruz.
         */
        if (isSliding || !IsGrounded())
            return;

        PlaySound("Jump");

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.VelocityChange
        );
    }

    private bool IsGrounded()
    {
        Bounds bounds = boxCollider.bounds;

        float rayDistance =
            bounds.extents.y +
            groundCheckDistance;

        return Physics.Raycast(
            bounds.center,
            Vector3.down,
            rayDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnCollisionEnter(
        Collision collision
    )
    {
        if (PlayerManager.gameOver)
            return;

        if (!collision.collider.CompareTag("Obstacle"))
            return;

        PlayerManager.SetGameOver();

        PlaySound("GameOver");
        StopSound("MainTheme");
    }

    private IEnumerator Slide()
    {
        isSliding = true;

        PlaySound("Slide");

        if (animator != null)
        {
            animator.SetBool(
                "isSliding",
                true
            );
        }

        Vector3 slideSize = normalColliderSize;

        slideSize.y =
            normalColliderSize.y *
            slideColliderHeightMultiplier;

        float removedHeight =
            normalColliderSize.y -
            slideSize.y;

        Vector3 slideCenter =
            normalColliderCenter +
            Vector3.down *
            (removedHeight * 0.5f);

        boxCollider.size = slideSize;
        boxCollider.center = slideCenter;

        yield return new WaitForSeconds(
            slideDuration
        );

        boxCollider.size = normalColliderSize;
        boxCollider.center = normalColliderCenter;

        if (animator != null)
        {
            animator.SetBool(
                "isSliding",
                false
            );
        }

        isSliding = false;
    }

    private void PlaySound(string soundName)
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;

        if (audioManager != null)
            audioManager.PlaySound(soundName);
    }

    private void StopSound(string soundName)
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;

        if (audioManager != null)
            audioManager.StopSound(soundName);
    }
}