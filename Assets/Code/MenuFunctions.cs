////////////////////////////
/// By: Gavin C
/// Date: 1/4/2021
/// Desription: This is a script for the menu
///////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [Tooltip("Name of the scene of the player should go to.")]
    public string SceneName;


    //Function to be called that loads new scene
    public void ChangeScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
        Debug.Log("Loading Scene: " + SceneName);
    }


    //Function to close game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
