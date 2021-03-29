//------------------------------------------------------------------------------
//
// File Name:	RandomSpawner.cs
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

public class RandomSpawner : MonoBehaviour
{
    [Tooltip("How often an object will try to spawn in, in seconds")]
    public Vector2 SpawnInterval = new Vector2(0, 1);
    [Tooltip("The objects that will be spawned in")]
    public GameObject[] ObjectPrefabs = null;
    [Tooltip("The chance the object of the same index will spawn in")]
    [Range(0, 1)]
    public float[] ObjectSpawnChances;

    // Private variables
    float timeTilNextSpawn = 0.0f;
    float StartingGameSpeed;
    float SpawnSpeed;

    //Runs before first frame
    void Start()
    {
        StartingGameSpeed = PlayerSaveData.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        //check that strings are valid length
        if (ObjectPrefabs.Length != ObjectSpawnChances.Length)
        {
            Debug.LogError("Diffrent lengths of ObjectPrefabs & ObjectSpawnChance on script RandomSpawner");
            return;
        }

        //get GameSpeed multiply
        SpawnSpeed = PlayerSaveData.Speed / StartingGameSpeed;

        //decrease time on timer
        timeTilNextSpawn -= Time.deltaTime * SpawnSpeed;

        //try spawning things if time is up
        if(timeTilNextSpawn <= 0.0f)
        {
            timeTilNextSpawn += Random.Range(SpawnInterval.x, SpawnInterval.y);
            SpawnRandomObject();
            //Debug.Log("SPAWN");
        }
    }

    void SpawnRandomObject()
    {
        // Determine spawn position
        Vector3 spawnPosition = transform.position;

        // Create object based on die roll
        for (int i = 0; i < ObjectPrefabs.Length; i++)
        {
            //variables
            GameObject spawnedObject = null;
            float RandomNumber;

            //get random number to compare to chance
            RandomNumber = Random.Range(0f, 1f);

            //check if object should be spawned
            if (ObjectSpawnChances[i] > RandomNumber)
            {
                //spawn object and set location
                spawnedObject = Instantiate(ObjectPrefabs[i]);
                spawnedObject.transform.position = spawnPosition;
            }
            
        }
    }
}
