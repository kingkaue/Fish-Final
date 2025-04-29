using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private TextMeshProUGUI finalScoreText;

    void Awake()
    {
        // Subscribe early
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // In case we started in the GameOver scene (and missed the callback)
        if (SceneManager.GetActiveScene().name == "GameOver")
            DisplayFinalScore();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOver")
            DisplayFinalScore();
    }

    private void DisplayFinalScore()
    {
        int xp = GameManager.Instance != null
              ? GameManager.Instance.GetXP()
              : 0;

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score\n{xp}";
    }
}