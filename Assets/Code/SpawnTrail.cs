//------------------------------------------------------------------------------
//
// File Name:	SpawnTrail.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that spawns a prefab at a location and sets it velocity to go left
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrail : MonoBehaviour
{
    [Tooltip("The object that will be summoned to create the trail")]
    public GameObject TrailEntity;
    [Tooltip("How often an entity will be spawned")]
    public float Interval = 0.5f;
    [Tooltip("Speed of entity moving left")]
    public float EntitySpeed = 10f;
    [Tooltip("The offset of the spawn location")]
    public Vector3 Offset;

    float StartingGameSpeed;
    float GameSpeedMultiplyer;

    // Start before first frame
    void Start()
    {
        StartingGameSpeed = PlayerSaveData.Speed;
        Invoke("SummonEntity", Interval);
    }

    void SummonEntity()
    {
        //get GameSpeed multiply
        GameSpeedMultiplyer = PlayerSaveData.Speed / StartingGameSpeed;

        //get objects 
        var TrailObject = Instantiate(TrailEntity);
        var PlayerLocation = GetComponent<Transform>();

        //set location
        TrailObject.transform.position = new Vector3(PlayerLocation.position.x + Offset.x, PlayerLocation.position.y + Offset.y, PlayerLocation.position.z + Offset.z);

        //set velocity
        TrailObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-EntitySpeed * GameSpeedMultiplyer, 0);

        //invoke function again
        Invoke("SummonEntity", Interval/GameSpeedMultiplyer); //not exactly what I would like but close
    }
}
