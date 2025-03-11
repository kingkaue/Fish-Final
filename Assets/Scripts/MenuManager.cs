using UnityEngine;

public class MenuManager : MonoBehaviour
{
    bool isPaused;
    public GameObject pauseUI;
    //GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //player = GameObject.Find("Player");
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseUI.SetActive(false);
        //player.SetActive(true);
    }

    void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseUI.SetActive(true);
        //player.SetActive(false);
    }
}
