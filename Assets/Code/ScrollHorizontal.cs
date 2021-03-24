//------------------------------------------------------------------------------
//
// File Name:	ScrollHorizontal.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
//              Gavin Cooper -added ability to offset platforms
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHorizontal : MonoBehaviour
{
    [Tooltip("If you want the platform to move the other way")]
    public bool FlipDirection = false;
    [Tooltip("The platform in front of this one in the ")]
    public GameObject PlatformInFront;
    [Tooltip("Speed the platform is moving")]
    public float MoveSpeed = 10.0f;
    [Tooltip("There the warp zone is on the left")]
    public float WrapZoneLeft = -18.0f;
    [Tooltip("There the warp zone is on the right")]
    public float WrapZoneRight = 56.0f;
    [Tooltip("This is the minimum variation there can be between the teleportation of platforms")]
    public float MinWarpVariationRange = 0;
    [Tooltip("This is the minimum variation there can be between the teleportation of platforms")]
    public float MaxWarpVariationRange = 0;
    [Tooltip("The most recent location the platform warped to")]
    public float MostRecentWarp = 0;
    

    public float MostRecentOffSet = 0; //public for debuging
    float StartingHeight;

    //set starting positions
    void Start()
    {
        //get the starting height
        StartingHeight = transform.position.y;

        //set the starting warp zones
        MostRecentWarp = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Get variables to use
        Vector3 position = transform.position;
        float PlatformInFrontWarp = PlatformInFront.GetComponent<ScrollHorizontal>().MostRecentWarp;
        float OffSet = Random.Range(MinWarpVariationRange, MaxWarpVariationRange);

        // Left --> Right, Reset
        if (FlipDirection)
        {
            if (transform.position.x >= WrapZoneRight)
            {
                //change platforms position based off the platform in fronts last warp & the random offset
                position.x = PlatformInFrontWarp - OffSet + MostRecentOffSet;

                //set variables for next platform to access
                MostRecentWarp = position.x;
                MostRecentOffSet = OffSet;
            }
        }
        // Left <-- Right, Reset
        else
        {
            if (transform.position.x <= WrapZoneLeft)
            {
                //change platforms position based off the platform in fronts last warp & the random offset
                position.x = PlatformInFrontWarp + OffSet - MostRecentOffSet;

                //set variables for next platform to access
                MostRecentWarp = position.x;
                MostRecentOffSet = OffSet;
            }
        }

        // Move
        if(FlipDirection)
        {
            position.x += MoveSpeed * Time.deltaTime;
        }
        else
        {
            position.x -= MoveSpeed * Time.deltaTime;
        }

        // Set new position
        transform.position = position;
    }
}
