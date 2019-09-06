using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingScript : MonoBehaviour {

    //Refrencing Canvas For Quality Level
    public Canvas Graphic;

    // Refrencing The built in unity Audio Mixer and Toggles for Full Screen and Volume
    public AudioMixer src;

    public Toggle volumeToggle;

    public Toggle windowToggle;

    
    void Start()
    {
        Graphic = Graphic.GetComponent<Canvas>();
       
        Graphic.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Low();
            Debug.Log("1");
        }
        if (Input.GetKeyDown("2"))
        {
            Med();
            Debug.Log("2");
        }
        if (Input.GetKeyDown("3"))
        {
            High();
            Debug.Log("3");
        }
    }

    //Quality Settings
    public void Low()
    {
        QualitySettings.SetQualityLevel(0);
    }
    public void Med()
    {
        QualitySettings.SetQualityLevel(1);
    }
    public void High()
    {
        QualitySettings.SetQualityLevel(3);
    }

    //Master Volume On/Off
    public void masterVolume()
    {
        if (volumeToggle.isOn)
        {
            src.SetFloat("Volume", 0);
        }
        else
        {
            src.SetFloat("Volume", -80);
        }

    }

    //Windowed/ FullScreenMode
    public void Window()
    {
        if (windowToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}

