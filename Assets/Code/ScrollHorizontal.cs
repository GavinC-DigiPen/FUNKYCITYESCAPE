//------------------------------------------------------------------------------
//
// File Name:	ScrollHorizontal.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
//              Gavin Cooper -added ability to offset platforms
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Note: The x possition of the center of the parent object needs to be alined with the left edge of the platform tilemap, also adde up and down movement
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
    [Tooltip("There the warp zone is on the left")]
    public float WrapZoneLeft = -18.0f;
    [Tooltip("This is the minimum and maximum variation there can be between the teleportation of platforms")]
    public Vector2 WarpVariationRange = new Vector2(0, 0);
    [Tooltip("This is the minimum and maximum variation there can be between the height of the platform evertime it teleports")]
    public Vector2 HeightVariationRange = new Vector2(0, 0);


    float MoveSpeed;
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
        // get rigidbody
        var PlatformRB = GetComponent<Rigidbody2D>();

        // Get variables to use
        MoveSpeed = PlayerSaveData.Speed;
        Vector3 position = transform.position;
        float FrontPosition = PlatformInFront.GetComponent<Transform>().position.x;
        float FrontLength = PlatformInFront.GetComponent<ScrollHorizontal>().Length;
        float XOffSet = Random.Range(WarpVariationRange.x, WarpVariationRange.y);
        float YOffSet = Random.Range(HeightVariationRange.x, HeightVariationRange.y);


        // Left <-- Right, Reset
        if (transform.position.x <= WrapZoneLeft)
        {
            //change platforms position based off the platform in fronts and add random offset
            position.x = FrontPosition + FrontLength + XOffSet;

            //change platforms height
            position.y = StartingHeight + YOffSet;

            //set position
            transform.position = position;
        }


        // Move
        PlatformRB.velocity = new Vector3(-MoveSpeed, PlatformRB.velocity.y);
    }
}
