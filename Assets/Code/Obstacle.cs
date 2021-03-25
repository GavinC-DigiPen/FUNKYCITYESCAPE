//------------------------------------------------------------------------------
//
// File Name:	Obstacle.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
//              Gavin Cooper
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool DestroyOnPlayerCollision = true;
    public int Damage = 1;
    public float DestroyXLimit = -19.0f;
    public float moveSpeed = -10.0f;

    private float yPosition = 0.0f;
    private Rigidbody2D physics = null;

    // Start is called before the first frame update
    void Start()
    {
        yPosition = transform.position.y;
        physics = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        physics.velocity = new Vector3(moveSpeed, 0, 0);
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

        if(transform.position.x <= DestroyXLimit)
        {
            Destroy(gameObject);
        }
    }
}
