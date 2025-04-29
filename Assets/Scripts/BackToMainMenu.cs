using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    
    public string mainMenuSceneName = "Menu";

    // Call this from the button's OnClick event
    public void LoadMainMenu()
    {
        // Reset time scale in case game was paused
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}