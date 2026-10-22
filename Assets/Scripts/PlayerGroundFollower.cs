using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerGroundFollower : MonoBehaviour
{
    [Header("Bağlantı")]
    [SerializeField] private Transform player;

    [Header("Platform Boyutları")]
    [SerializeField] private float platformWidth = 15f;
    [SerializeField] private float platformLength = 30f;
    [SerializeField] private float platformThickness = 0.2f;

    [Header("Konum")]
    [Tooltip("Yolun merkez X konumu")]
    [SerializeField] private float roadCenterX = 0f;

    [Tooltip("Platform merkezinin Y konumu")]
    [SerializeField] private float platformY = -0.1f;

    [Tooltip("Platform oyuncunun biraz önünde dursun")]
    [SerializeField] private float forwardOffset = 3f;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        boxCollider.size = new Vector3(
            platformWidth,
            platformThickness,
            platformLength
        );

        boxCollider.center = Vector3.zero;
    }

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError(
                "PlayerGroundFollower: Player atanmamış.",
                this
            );

            enabled = false;
            return;
        }

        Vector3 startPosition = new Vector3(
            roadCenterX,
            platformY,
            player.position.z + forwardOffset
        );

        rb.position = startPosition;
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        Vector3 targetPosition = new Vector3(
            roadCenterX,
            platformY,
            player.position.z + forwardOffset
        );

        rb.MovePosition(targetPosition);
    }
}