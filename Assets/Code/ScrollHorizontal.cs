//------------------------------------------------------------------------------
//
// File Name:	ScrollHorizontal.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
//              Gavin Cooper -added ability to offset platforms
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Note: The x possition of the center of the parent object needs to be alined with the left edge of the platform tilemap
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHorizontal : MonoBehaviour
{
    [Tooltip("The platform in front of this one")]
    public GameObject PlatformInFront;
    [Tooltip("The length of the platform")]
    public float Length = 0;
    [Tooltip("Speed the platform is moving")]
    public float MoveSpeed = 10.0f;
    [Tooltip("There the warp zone is on the left")]
    public float WrapZoneLeft = -18.0f;
    [Tooltip("This is the minimum and maximum variation there can be between the teleportation of platforms")]
    public Vector2 WarpVariationRange = new Vector2(0, 0);

    

    public float MostRecentOffSet = 0; //public for debuging
    float StartingHeight;

    //set starting positions
    void Start()
    {
        //get the starting height
        StartingHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Get variables to use
        Vector3 position = transform.position;
        float FrontPosition = PlatformInFront.GetComponent<Transform>().position.x;
        float FrontLength = PlatformInFront.GetComponent<ScrollHorizontal>().Length;
        float OffSet = Random.Range(WarpVariationRange.x, WarpVariationRange.y);


        // Left <-- Right, Reset
        if (transform.position.x <= WrapZoneLeft)
        {
            //change platforms position based off the platform in fronts and add random offset
            position.x = FrontPosition + FrontLength + OffSet;
        }


        // Move
        position.x -= MoveSpeed * Time.deltaTime;


        // Set new position
        transform.position = position;
    }
}
