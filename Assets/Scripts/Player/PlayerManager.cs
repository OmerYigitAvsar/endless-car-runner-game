using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver { get; private set; }
    public static bool isGameStarted { get; private set; }
    public static int numberOfCoins { get; private set; }
    public static int currentScore { get; private set; }

    [Header("Oyuncu ve Skor")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("Oyuncunun ilerlediği her 1 Unity birimi için verilecek puan.")]
    [SerializeField, Min(0.01f)] private float pointsPerUnit = 1f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverCoinsText;

    [Header("Başlangıç")]
    [SerializeField] private GameObject startingText;

    [Header("Oyun İçi UI")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private bool gameOverHandled;
    private bool scoreTrackingStarted;

    private float scoreStartZ;

    private int lastDisplayedCoinCount = -1;
    private int lastDisplayedScore = -1;

    private void Start()
    {
        ResetGameState();

        gameOverHandled = false;
        scoreTrackingStarted = false;

        lastDisplayedCoinCount = -1;
        lastDisplayedScore = -1;

        Time.timeScale = 1f;

        FindPlayerIfNeeded();

        if (playerTransform != null)
        {
            scoreStartZ = playerTransform.position.z;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (startingText != null)
        {
            startingText.SetActive(true);
        }

        SetGameplayTextsActive(true);
        UpdateGameplayTexts(true);
        UpdateGameOverTexts();
    }

    private void Update()
    {
        if (!gameOver &&
            !isGameStarted &&
            SwipeManager.tap)
        {
            StartGame();
        }

        if (isGameStarted && !gameOver)
        {
            UpdateScoreFromDistance();
        }

        if (gameOver && !gameOverHandled)
        {
            HandleGameOver();
        }

        UpdateGameplayTexts();
    }

    private void StartGame()
    {
        isGameStarted = true;
        scoreTrackingStarted = true;
        currentScore = 0;

        FindPlayerIfNeeded();

        /*
         * Oyuncunun oyuna başladığı Z konumunu
         * skorun başlangıç noktası olarak alıyoruz.
         */
        if (playerTransform != null)
        {
            scoreStartZ = playerTransform.position.z;
        }

        if (startingText != null)
        {
            startingText.SetActive(false);
        }

        UpdateGameplayTexts(true);
    }

    private void UpdateScoreFromDistance()
    {
        if (!scoreTrackingStarted ||
            playerTransform == null)
        {
            return;
        }

        float travelledDistance =
            playerTransform.position.z - scoreStartZ;

        int calculatedScore = Mathf.Max(
            0,
            Mathf.FloorToInt(
                travelledDistance * pointsPerUnit
            )
        );

        /*
         * Oyuncu herhangi bir nedenle geri giderse
         * skorun azalmasını engeller.
         */
        if (calculatedScore > currentScore)
        {
            currentScore = calculatedScore;
        }
    }

    private void HandleGameOver()
    {
        gameOverHandled = true;

        /*
         * Çarpışmanın gerçekleştiği son konumu
         * da skora dahil ediyoruz.
         */
        UpdateScoreFromDistance();

        UpdateGameOverTexts();

        /*
         * Oyun sırasında görünen coin ve skor
         * yazılarını kapatıyoruz.
         */
        SetGameplayTextsActive(false);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void UpdateGameplayTexts(
        bool forceUpdate = false
    )
    {
        if (coinsText != null &&
            (forceUpdate ||
             lastDisplayedCoinCount != numberOfCoins))
        {
            lastDisplayedCoinCount = numberOfCoins;

            coinsText.text =
                $"Coins: {numberOfCoins}";
        }

        if (scoreText != null &&
            (forceUpdate ||
             lastDisplayedScore != currentScore))
        {
            lastDisplayedScore = currentScore;

            scoreText.text = currentScore.ToString();
        }
    }

    private void UpdateGameOverTexts()
    {
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text =
                $"Score: {currentScore}";
        }

        if (gameOverCoinsText != null)
        {
            gameOverCoinsText.text =
                $"Coins: {numberOfCoins}";
        }
    }

    private void SetGameplayTextsActive(
        bool isActive
    )
    {
        if (coinsText != null)
        {
            coinsText.gameObject.SetActive(isActive);
        }

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(isActive);
        }
    }

    private void FindPlayerIfNeeded()
    {
        if (playerTransform != null)
        {
            return;
        }

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            return;
        }

        Debug.LogWarning(
            "PlayerManager: Player Transform atanmamış " +
            "ve Player tag'li obje bulunamadı.",
            this
        );
    }

    public static void AddCoin(int amount = 1)
    {
        if (amount <= 0 || gameOver)
        {
            return;
        }

        numberOfCoins += amount;
    }

    public static void SetGameOver()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;
    }

    public static void ResetGameState()
    {
        gameOver = false;
        isGameStarted = false;

        numberOfCoins = 0;
        currentScore = 0;
    }
}