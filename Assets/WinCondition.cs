using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string gameOverSceneName = "Game Over";
    [SerializeField] private EnemyStats bossStats; // Assign in Inspector

    private bool _hasTriggeredGameOver = false;

    private void Awake()
    {
        // Auto-get EnemyStats if not assigned
        if (bossStats == null)
        {
            bossStats = GetComponent<EnemyStats>();
            if (bossStats == null)
            {
                Debug.LogError("No EnemyStats found on boss!", this);
            }
        }
    }

    private void Update()
    {
        // Check if boss health is 0 and game over hasn't been triggered yet
        if (!_hasTriggeredGameOver && bossStats != null && bossStats.CurrentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        _hasTriggeredGameOver = true;
        Debug.Log("Boss defeated! Loading Game Over scene...");

        // Load the game over scene
        SceneManager.LoadScene(gameOverSceneName);
    }

}
