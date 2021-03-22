//------------------------------------------------------------------------------
//
// File Name:	MenuButton.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonFunctions
{
    Play,
    Exit,
    Restart,
    MainMenu
}

public class MenuButton : MonoBehaviour
{
    // Options for what to do when button is clicked
    public ButtonFunctions buttonFunction;

    // Function to call when button is clicked
    private delegate void ButtonAction();
    private ButtonAction buttonAction;

    // Start is called before the first frame update
    void Start()
    {
        buttonAction = null;

        switch(buttonFunction)
        {
            case ButtonFunctions.Play:
                buttonAction = Play;
                break;
            case ButtonFunctions.Exit:
                buttonAction = Exit;
                break;
            case ButtonFunctions.Restart:
                buttonAction = Restart;
                break;
            case ButtonFunctions.MainMenu:
                buttonAction = MainMenu;
                break;
        }
    }

    public void OnButtonClicked()
    {
        buttonAction();
    }

    private void Play()
    {
        SceneManager.LoadScene("GameLevel");
    }

    private void Exit()
    {
    #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
