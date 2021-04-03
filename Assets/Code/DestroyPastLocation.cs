//------------------------------------------------------------------------------
//
// File Name:	DestroyPastLocation.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that destorys the game object when it goes to far left
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPastLocation : MonoBehaviour
{
    [Tooltip("The point at which the object is destoyed")]
    public float DestroyXLimit = -25.0f;

    // Update is called once per frame
    void Update()
    {
        //check if object passed point, if so, destroy
        if (transform.position.x <= DestroyXLimit)
        {
            Destroy(gameObject);
        }
    }
}
