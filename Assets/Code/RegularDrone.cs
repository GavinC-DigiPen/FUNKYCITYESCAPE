//------------------------------------------------------------------------------
//
// File Name:	RegularDrone.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that goes on a drone to give it a random height and make it move
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDrone : MonoBehaviour
{
    [Tooltip("The speed multiply of the obstacle (1 is the same as enviorment)")]
    public float SpeedMultiply = 1;
    [Tooltip("The range of heights that the drone can spawn in")]
    public Vector2 HeightRange;
    [Tooltip("The range of horizontal offset on the drone")]
    public Vector2 HorizontalOffset;

    float GameSpeed;
    private Rigidbody2D DroneRB = null;

    // Start is called before the first frame update
    void Start()
    {
        //get RB of drone
        DroneRB = gameObject.GetComponent<Rigidbody2D>();

        //give drone random height
        transform.position = new Vector3(transform.position.x + Random.Range(HorizontalOffset.x, HorizontalOffset.y), Random.Range(HeightRange.x, HeightRange.y), transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //move drone
        GameSpeed = PlayerSaveData.Speed;
        DroneRB.velocity = new Vector3(-GameSpeed * SpeedMultiply, 0, 0);
    }
}
