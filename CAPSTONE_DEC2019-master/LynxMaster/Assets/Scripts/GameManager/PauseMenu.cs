using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused;

    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameIsPaused || Input.GetButtonDown("Start") && GameIsPaused)
        {
            Resume();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !GameIsPaused || Input.GetButtonDown("Start") && !GameIsPaused)
        {
            Pause();
        }
    }

     void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


}
