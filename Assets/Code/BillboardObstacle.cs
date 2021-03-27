//------------------------------------------------------------------------------
//
// File Name:	BillboardObstacle.cs
// Author(s):	Gavin Cooper
//
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Description: A script that handles the movement of the billboard
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardObstacle : MonoBehaviour
{
    [Tooltip("The diffrent heights the billboard can spawn at")]
    public int[] Heights = {0};

    float GameSpeed;
    private Rigidbody2D BillboardRB = null;

    // Start is called before the first frame update
    void Start()
    {
        //get Rigidbody
        BillboardRB = gameObject.GetComponent<Rigidbody2D>();

        //change height by random amount
        var NewHeight = Heights[Random.Range(0, Heights.Length)];
        transform.position = new Vector3(transform.position.x, NewHeight, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //get game speed
        GameSpeed = PlayerSaveData.Speed;

        //add velocity to object
        BillboardRB.velocity = new Vector3(-GameSpeed, 0, 0);
    }
}
