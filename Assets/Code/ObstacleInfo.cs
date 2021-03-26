//------------------------------------------------------------------------------
//
// File Name:	ObstacleInfo.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that defines how much damage this obstacle does & if it get destoyed, without his script it is 1
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
    [Tooltip("How much damage does the player take on contact")]
    public int Damage = 1;
    [Tooltip("Does the object get destroyed on contact")]
    public bool DestroyOnPlayerCollision = true;
}
