////////////////////////////
/// By: Gavin C
/// Date: 1/4/2021
/// Desription: This is a script for the pause menu
///////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseFunction : MonoBehaviour
{
    [Tooltip("The state of pause.")]
    public static bool IsPaused = false;
    [Tooltip("The parent object of the pause menu (not the object this script is on).")]
    public GameObject PauseMenu;


    //make sure pause is off
    void Start()
    {
        IsPaused = false;
    }


    // Update to check if pause button is pushed
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsPaused)
            {
                Resume();
            }

            else if (!IsPaused)
            {
                Pause();
            }
        }
    }


    //Function to pause game
    public void Pause()
    {
        Time.timeScale = 0;
        IsPaused = true;
        PauseMenu.SetActive(true);
    }


    //Fuction to resume game
    public void Resume()
    {
        Time.timeScale = 1;
        IsPaused = false;
        PauseMenu.SetActive(false);
    }
}
