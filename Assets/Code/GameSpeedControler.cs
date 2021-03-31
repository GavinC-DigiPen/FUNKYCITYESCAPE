//------------------------------------------------------------------------------
//
// File Name:	GameSpeedControler.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that changes the speed of the level
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
    [Tooltip("The object that is the indicator")]
    public GameObject Indicator;
    [Tooltip("The diffrent sprite that show speed")]
    public Sprite[] IndicatorSprites;
    
    int SpeedIndex = 0;
    int SpriteIndex;
    float DelayCounter = 0;

    // Start is called before the first frame update
    void Start()
    {

        PlayerSaveData.Speed = Speeds[0];
    }

    // Update is called once per frame
    void Update()
    {
        //count down on the timer
        DelayCounter =- Time.deltaTime;

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
                DelayCounter = Delay;

                //get correct sprite index
                SpriteIndex = SpeedIndex;
                if (SpriteIndex >= IndicatorSprites.Length)
                {
                    SpriteIndex = IndicatorSprites.Length - 1;
                }

                //change sprite of speed idicator
                Indicator.GetComponent<SpriteRenderer>().sprite = IndicatorSprites[SpeedIndex];
            }
        }
    }
}
