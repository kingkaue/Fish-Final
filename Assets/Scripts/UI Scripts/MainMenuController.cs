using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public CanvasGroup LevelSelect;

    public void PlayGame()
    {
        LevelSelect.alpha = 1;
        LevelSelect.interactable = true;
        LevelSelect.blocksRaycasts = true;
    }

    public void Mage()
    {
        SceneManager.LoadScene("FrozenLevel");
    }

    public void Brawler()
    {
        SceneManager.LoadScene("ForestLevel");
    }

    public void Rogue()
    {
        SceneManager.LoadScene("SandLevel");
    }

    public void Option()
    {
        // TODO LOAD STATE
    }
    public void Back()
    {

        LevelSelect.alpha = 0;
        LevelSelect.blocksRaycasts = false;
        LevelSelect.interactable = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
