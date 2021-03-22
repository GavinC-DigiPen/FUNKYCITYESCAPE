//------------------------------------------------------------------------------
//
// File Name:	Obstacle.cs
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

public class Obstacle : MonoBehaviour
{
    public bool DestroyOnPlayerCollision = true;
    public int Damage = 1;
    public float DestroyXLimit = -10.0f;

    private float yPosition = 0.0f;
    private GameObject player = null;
    private Rigidbody2D physics = null;

    // Start is called before the first frame update
    void Start()
    {
        yPosition = transform.position.y;
        player = GameObject.Find("Player");
        physics = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = -10.0f;

        if(player != null)
        {
            moveSpeed = -player.GetComponent<PlayerMovementController>().MoveSpeed;
        }

        physics.velocity = new Vector3(moveSpeed, 0, 0);
        transform.position = new Vector3(transform.position.x, 
            yPosition, transform.position.z);

        if(transform.position.x <= DestroyXLimit)
        {
            Destroy(gameObject);
        }
    }
}
