using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public CanvasGroup PauseMenu;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Back();
            else Pause();
        }
    }

    public void Pause()
    {
        PauseMenu.alpha = 1f;
        PauseMenu.blocksRaycasts = true;
        PauseMenu.interactable = true;

        Time.timeScale = 0f;    // freeze everything
        isPaused = true;
    }

    public void Back()
    {
        PauseMenu.alpha = 0f;
        PauseMenu.blocksRaycasts = false;
        PauseMenu.interactable = false;

        Time.timeScale = 1f;    // resume normal time
        isPaused = false;
    }

    public void Menu()
    {
        Time.timeScale = 1f;    // ensure time is running when you leave
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
