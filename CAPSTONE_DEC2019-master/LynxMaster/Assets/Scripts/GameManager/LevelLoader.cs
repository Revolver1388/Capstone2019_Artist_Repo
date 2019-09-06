using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject m_LoadScreen;
    public Slider progressBar;
    public Text progressPercent;

    //Starts the corutoutine to start the level
    public void LoadLevel(int LevelIndex)
    {
        StartCoroutine(LoadAsynchronously(LevelIndex));
    }

    //Where the funtcionality for loading/loadingScreen is
    IEnumerator LoadAsynchronously(int LevelIndex)
    {
        //loads the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelIndex);

        //Enables the load screen so you can see it
        m_LoadScreen.SetActive(true);

        //While the level is still load do Something
        while(!operation.isDone)
        {
            //Unity is weird with the way it records progress. this is just to give us a nice 0 to 1 float value to work with
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
           
            // this sets the progress slider to whatever the percentage of completion the operation is currently at
            progressBar.value = progress;
            //this sets the text to whatever the percentage of completion the operation is currently at
            progressPercent.text = progress * 100f + "%";
            //waits a frame before continuing
            yield return null;
        }
    }
}
