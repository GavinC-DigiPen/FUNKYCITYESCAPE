//------------------------------------------------------------------------------
//
// File Name:	SettingsMenu.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that is used for settings menu functions
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Tooltip("The audio mixer for the game.")]
    public AudioMixer Mixer;
    [Tooltip("The resolution dropdown.")]
    public Dropdown ResolutionDropdown;
    [Tooltip("The volume slider.")]
    public Slider VolumeSlider;

    Resolution[] Resolutions;
    float CurrentVolume;


    //Set up the resolution dropdown
    void Start()
    {
        //set volume slider
        if (VolumeSlider)
        {
            Mixer.GetFloat("Volume", out CurrentVolume);
            VolumeSlider.value = Mathf.Pow(10, CurrentVolume / 20);
        }

        //deal with resolution 
        if (ResolutionDropdown)
        {
            //set up resolution and clear dropdown
            Resolutions = Screen.resolutions;
            ResolutionDropdown.ClearOptions();

            //list to store dropdown options
            List<string> DropdownOptions = new List<string>();

            //add each resolution to the list
            int CurrentResolutionIndex = 0;
            for (int i = 0; i < Resolutions.Length; i++)
            {
                string Option = Resolutions[i].width + " x " + Resolutions[i].height;
                DropdownOptions.Add(Option);

                if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
                {
                    CurrentResolutionIndex = i;
                }
            }

            //set the dropdown to the list
            ResolutionDropdown.AddOptions(DropdownOptions);

            //Set resolution
            ResolutionDropdown.value = CurrentResolutionIndex;
            ResolutionDropdown.RefreshShownValue();
        }
    }


    //Function to change resolution
    public void SetResolution(int ResolutionIndex)
    {
        Resolution Resolution = Resolutions[ResolutionIndex];
        Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreen);
    }


    //Function to set volume
    public void SetVolume(float volume)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        Debug.Log("Set volume: " + volume);
    }


    //Function to set fullscreen
    public void SetFullscreen(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;
        Debug.Log("Change fullscreen to: " + IsFullscreen);
    }
}
