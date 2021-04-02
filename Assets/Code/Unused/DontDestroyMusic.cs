//------------------------------------------------------------------------------
//
// File Name:	DontDestroyMusic.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that goes on an object that is playing the game music and keeps it alive through scene loads
// Credit:      https://www.youtube.com/watch?v=JKoBWBXVvKY&t
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    // Runs when object awakes
    void Awake()
    {
        //destory if other music already exists
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        //keep alive forever
        DontDestroyOnLoad(this.gameObject);
    }
}
