//------------------------------------------------------------------------------
//
// File Name:	GameSpeedControler.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that changes the speed of the game
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedControler : MonoBehaviour
{
    [Tooltip("The button to be pushed ")]
    public KeyCode ChangeSpeedButton = KeyCode.Q;
    [Tooltip("The object that will be summoned to create the trail")]
    public float[] Speeds;
    [Tooltip("The delay between when you can change speed")]
    public float Delay = 1;
    [Tooltip("The persent of the original speed that will be added ever 100 distance")]
    public float DifficultlyMultiplier = 0.1f;
    [Tooltip("The object that is the indicator")]
    public GameObject Indicator;
    [Tooltip("The diffrent speed sprites")]
    public Sprite[] IndicatorSprites;
    [Tooltip("The diffrent speed sounds")]
    public AudioClip[] AudioIndicators;

    AudioSource audioSource;
    float[] StartingSpeeds = {0, 0};
    int SpeedIndex = 0;
    int SpriteIndex;
    int AudioIndex;
    float DelayCounter = 0;
    int DistanceCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //get audio source
        audioSource = GetComponent<AudioSource>();

        //set the speed
        PlayerSaveData.Speed = Speeds[0];

        //get copy of starting speeds
        for (int i = 0; i < Speeds.Length; i++)
        {
            StartingSpeeds[i] = Speeds[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(DelayCounter);
        //count down on the timer
        DelayCounter -= Time.deltaTime;

        //check if key was pressed to change speed
        if (Input.GetKeyDown(ChangeSpeedButton))
        {
            //only run if delay is over
            if (DelayCounter <= 0)
            {
                //increase the index and set to 0 if needed
                SpeedIndex++;
                if (SpeedIndex >= Speeds.Length)
                {
                    SpeedIndex = 0;
                }

                //change the game speed
                PlayerSaveData.Speed = Speeds[SpeedIndex];

                //set delay
                Debug.Log(DelayCounter);
                DelayCounter = Delay;
                Debug.Log(DelayCounter);

                //get correct sprite index
                SpriteIndex = SpeedIndex;
                if (SpriteIndex >= IndicatorSprites.Length)
                {
                    SpriteIndex = IndicatorSprites.Length - 1;
                }

                //change sprite of speed idicator
                Indicator.GetComponent<SpriteRenderer>().sprite = IndicatorSprites[SpriteIndex];

                //get correct sprite index
                AudioIndex = SpeedIndex;
                if (AudioIndex >= AudioIndicators.Length)
                {
                    AudioIndex = AudioIndicators.Length - 1;
                }

                //play sound
                audioSource.clip = AudioIndicators[AudioIndex];
                audioSource.Play();
            }
        }

        //check if game speed needs to be increase because a distance has been traveled
        if ((int)(PlayerSaveData.DistanceRun / 100) > DistanceCounter)
        {
            //add speed
            for (int i = 0; i < Speeds.Length; i++)
            {
                //increase current speed
                if (PlayerSaveData.Speed == Speeds[i])
                {
                    PlayerSaveData.Speed = StartingSpeeds[i] * DifficultlyMultiplier;
                }

                //increase speed list
                Speeds[i] = StartingSpeeds[i] * DifficultlyMultiplier;
            }

            //increase counter
            DistanceCounter++;
        }    
    }
}
