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
            if (isPaused)
            {
                Back();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PauseMenu.alpha = 1;
        PauseMenu.blocksRaycasts = true;
        isPaused = true;
    }

    public void Back()
    {
        PauseMenu.alpha = 0;
        PauseMenu.blocksRaycasts = false;
        isPaused = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
