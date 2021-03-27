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

    // Object prefabs and spawn chances
    public GameObject[] ObjectPrefabs = null;
    public float[] ObjectSpawnChances;

    // Private variables
    private float timeTilNextSpawn = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (ObjectPrefabs.Length != ObjectSpawnChances.Length)
        {
            Debug.LogError("Diffrent lengths of ObjectPrefabs & ObjectSpawnChance on script RandomSpawner");
            return;
        }

        timeTilNextSpawn -= Time.deltaTime;

        if(timeTilNextSpawn <= 0.0f)
        {
            timeTilNextSpawn += Random.Range(SpawnInterval.x, SpawnInterval.y);
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        // Determine spawn position
        Vector3 spawnPosition = transform.position;

        GameObject spawnedObject = null;

        // Create object based on die roll
        

        // Set object position
        spawnedObject.transform.position = spawnPosition;
    }
}
