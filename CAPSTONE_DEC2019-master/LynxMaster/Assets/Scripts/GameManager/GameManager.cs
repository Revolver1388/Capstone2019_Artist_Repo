//Made By Benjamin Young.  Purpose: Game Manager For Capstone Project. Last updated 17/05/2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool GameIsPaused;

    bool IsDead = false;

    public GameObject pauseMenuUI;

    bool isMuted = false;

    public int PlayerHealth;
    
    //public List<Image> Hearts = new List<Image>();

    public AudioSource Music;

    public GameObject UIElements;

    public Slider Volume;

    private float previousVolume;

    //Loading Screen Variables
    public GameObject m_LoadScreen;
    public Slider progressBar;
    public Text progressPercent;

    void Awake()
    {
        DontDestroyOnLoad(this);
        MakeSingleton();
        GameIsPaused = false;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeQuality();

        //LoseHealth();

        //HealthBar();

        //MuteSound();

        //Sets the sound volume to the slider within the pause menu
        //if(!isMuted)
        //{
        //    Music.volume = Volume.value;
        //}

        //Quits the game
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Quit();
        }
        //Unpauses the game
        if (Input.GetKeyDown(KeyCode.Escape) && GameIsPaused)
        {
            Resume();
        }
        //Pauses the Game
        else if (Input.GetKeyDown(KeyCode.Escape) && !GameIsPaused)
        {
            Pause();
        }
    }
    //Makes sure that there is only ever one gameManager being used in a scence;
    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //Placeholder for quitting the application 
    public void Quit()
    {
        Debug.Log("Game is Quitting...");
        Application.Quit();

    }


    //simply changes the quality based on unitys standerds of Low, Medium and High quality
    public void ChangeQuality()
    {
        switch(Input.inputString)
        {
            case "1":
                QualitySettings.SetQualityLevel(0, true);
                Debug.Log("Quality set to Low");
                break;


            case "2":
                QualitySettings.SetQualityLevel(2, true);
                Debug.Log("Quality set to Medium");
                break;

            case "3":
                QualitySettings.SetQualityLevel(3, true);
                Debug.Log("Quality set to High");
                break;


        }
    }

    //Controls the muting of sound in the scene
    public void MuteSound()
    {
        if(Input.GetKeyDown(KeyCode.M) && !isMuted)
        {
            isMuted = true;
            previousVolume = Music.volume;
            Music.volume = 0;
        }
        else if(Input.GetKeyDown(KeyCode.M) && isMuted)
        {
            
            Music.volume = previousVolume;
            isMuted = false;
        }
    }
    //Enables the pause menu and sets timescale to 0
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        UIElements.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }
    //Resumes timescale and dissables the Pause menu
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        UIElements.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    //Placeholder way for me to test health loss
    public void LoseHealth()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            PlayerHealth--;
        }
    }

    //Controlls the number of hearts on screen based on player health
    //public void HealthBar()
    //{

    //    if (PlayerHealth == 2)
    //    {
    //        Hearts[2].enabled = false;
    //    }

    //    else if (PlayerHealth == 1)
    //    {
    //        Hearts[1].enabled = false;
    //    }

    //    else if (PlayerHealth <= 0)
    //    {
    //        Hearts[0].enabled = false;
    //        Death();
    //        PlayerHealth = 3;
    //        IsDead = true;
    //    }
    //    //Restarts the game when the player dies
    //    else if (PlayerHealth == 3 && IsDead)
    //    {
    //        for (int i = 0; i < Hearts.Count ; i++)
    //        {
    //            Hearts[i].enabled = true;
    //        }
    //        IsDead = false;
    //    }
    //}

    //Placeholder for death 
    public void Death()
    {
        Debug.Log("You Died");
    }


 

   
   
}
