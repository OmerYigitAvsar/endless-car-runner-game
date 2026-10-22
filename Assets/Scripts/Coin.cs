using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private AudioManager audioManager;

    private Collider coinCollider;
    private bool collected;

    private void Awake()
    {
        coinCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        transform.Rotate(
            rotationSpeed * Time.deltaTime,
            0f,
            0f,
            Space.Self
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected ||
            !other.CompareTag("Player"))
        {
            return;
        }

        collected = true;

        if (coinCollider != null)
            coinCollider.enabled = false;

        if (audioManager == null)
            audioManager = AudioManager.Instance;

        if (audioManager != null)
        {
            audioManager.PlaySound(
                "PickUpCoin"
            );
        }

        PlayerManager.AddCoin();

        Destroy(gameObject);
    }
}