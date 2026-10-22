using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName = "Level01";

    [SerializeField]
    private string menuSceneName = "Menu";

    public void ReplayGame()
    {
        Time.timeScale = 1f;

        PlayerManager.ResetGameState();

        SceneManager.LoadScene(
            gameSceneName
        );
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;

        PlayerManager.ResetGameState();

        SceneManager.LoadScene(
            menuSceneName
        );
    }

    /*
     * Eski UI butonu QuitGame metoduna bağlıysa
     * bağlantının bozulmaması için bırakıldı.
     */
    public void QuitGame()
    {
        GoToMenu();
    }

    public void ExitApplication()
    {
        Time.timeScale = 1f;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}