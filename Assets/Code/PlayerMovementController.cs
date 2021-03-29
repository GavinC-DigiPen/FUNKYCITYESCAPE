﻿//------------------------------------------------------------------------------
//
// File Name:	PlayerMovementController.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
//              Gavin Cooper - added player movement side to side
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementController : MonoBehaviour
{
    [Tooltip("The health bar feedback bar")]
    public GameObject healthBarObj;
    [Tooltip("The distance text mesh pro object")]
    public GameObject distanceObj;
    [Tooltip("The prefab that is summoned during an attack to do damage")]
    public GameObject AttackObj;
    [Tooltip("The speed of the player's sprint")]
    public float MovementSpeed = 5;
    [Tooltip("The max and starting health")]
    public int MaxHealth = 3;
    [Tooltip("The jump height of the player, in unity squares")]
    public float JumpHeight = 5;
    [Tooltip("The number of jumps the player has")]
    public int MaxNumberOfJumps = 2;
    [Tooltip("The speed the player slams back to the ground")]
    public int SlamSpeed = 15;
    [Tooltip("The cool down between attacks")]
    public float AttackCooldown = 3;
    [Tooltip("The key that will be used for jumping")]
    public KeyCode JumpKey = KeyCode.Space;
    [Tooltip("The key that will be used for attacking")]
    public KeyCode AttackKey = KeyCode.E;
    [Tooltip("The key that will be used for sliding")]
    public KeyCode Slide_SlamKey = KeyCode.S;
    [Tooltip("The key that will be used for moving right")]
    public KeyCode RightKey = KeyCode.D;
    [Tooltip("The key that will be used for moving left")]
    public KeyCode LeftKey = KeyCode.A;
    [Tooltip("The box collider offset while sliding/attacking")]
    public Vector2 SlidingColliderOffset;
    [Tooltip("The box collider size while sliding/attacking")]
    public Vector2 SlidingColliderSize;

    float GameSpeed;
    int jumpsRemaining = 0;
    int currentHealth = 0;
    PlayerAnimationManager animationManager;
    BoxCollider2D PlayerBoxCollider;
    Rigidbody2D PlayerRB;
    Vector2 StartingColliderOffset;
    Vector2 StartingColliderSize;
    float Dirrection = 0;
    bool grounded = true;
    float AttackCooldownTimer;

    // Start is called before the first frame update
    void Start()
    { 
        //get components 
        animationManager = GetComponent<PlayerAnimationManager>();
        PlayerBoxCollider = GetComponent<BoxCollider2D>();
        PlayerRB = GetComponent<Rigidbody2D>();

        //set variables
        if (healthBarObj != null)
        {
          healthBarObj.GetComponent<FeedbackBar>().SetMax(MaxHealth);
        }
        currentHealth = MaxHealth;
        PlayerSaveData.DistanceRun = 0;
        JumpHeight = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * JumpHeight); // Take the square root of the jump height so that the math for gravity works to make the number the user enters the number of units the player will actually be able to jump
        StartingColliderOffset = PlayerBoxCollider.offset;
        StartingColliderSize = PlayerBoxCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        //set new speed
        GameSpeed = PlayerSaveData.Speed;

        //reset player hitbox
        PlayerBoxCollider.offset = StartingColliderOffset;
        PlayerBoxCollider.size = StartingColliderSize;

        // Jumping
        if (Input.GetKeyDown(JumpKey))
        {
            if (jumpsRemaining > 0)
            {
                animationManager.SwitchTo(PlayerAnimationStates.Jump);
                var jump_vec = new Vector3(transform.position.x,JumpHeight,0);
                PlayerRB.velocity = jump_vec;
                jumpsRemaining -= 1;
            }
        }
        //Slam
        else if (Input.GetKey(Slide_SlamKey) && !grounded && PlayerRB.velocity.y <= 0)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Slam);
            var slam_vec = new Vector3(transform.position.x, -SlamSpeed, 0);
            PlayerRB.velocity = slam_vec;
        }
        // Falling
        else if (!grounded)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Jump);
        }
        // Sliding
        else if (Input.GetKey(Slide_SlamKey) && grounded)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Slide);

            //change character hitbox
            PlayerBoxCollider.offset = SlidingColliderOffset;
            PlayerBoxCollider.size = SlidingColliderSize;

        }
        // Running
        else 
        {
            animationManager.SwitchTo(PlayerAnimationStates.Run);
        }

        //Count down attack cooldown
        AttackCooldownTimer -= Time.deltaTime;

        //Attacking
        if (Input.GetKey(AttackKey) && AttackCooldownTimer <= 0)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Attack);

            //change character hitbox
            PlayerBoxCollider.offset = SlidingColliderOffset;
            PlayerBoxCollider.size = SlidingColliderSize;

            //set cooldown time
            AttackCooldownTimer = AttackCooldown;
        }
        
        //Figure out the dirrection player is moving
        if (Input.GetKeyDown(LeftKey))
        {
            Dirrection = -1.5f;
        }
        if (Input.GetKeyDown(RightKey))
        {
            Dirrection = 1;
        }
        if (Input.GetKey(RightKey) == false && Input.GetKey(LeftKey) == false)
        {
            Dirrection = 0;
        }

        //Changes the players horizontal movement
        PlayerRB.velocity = new Vector2(Dirrection * MovementSpeed, PlayerRB.velocity.y);

        // Update the Distance travelled
        PlayerSaveData.DistanceRun += GameSpeed * Time.deltaTime;
        if (distanceObj != null)
        {
            if (distanceObj.GetComponent<TextMeshProUGUI>() != null)
            {
                string distText = string.Format("{0,4:F1}", PlayerSaveData.DistanceRun);
                distanceObj.GetComponent<TextMeshProUGUI>().text = "Distance: " 
                    + distText + " m";
            }
        }
    }

    //check if collide with an obstacle
    private void OnCollisionStay2D(Collision2D collision)
    {
        var InvulnTime = GetComponent<PlayerAnimationManager>().CurrInvulnTime;

        if (InvulnTime <= 0)
        {
            // Hit an Obstacle
            if (collision.collider.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("test1");
                ObstacleInfo Obstacle = collision.gameObject.GetComponent<ObstacleInfo>();

                if (Obstacle != null)
                {
                    currentHealth -= Obstacle.Damage;

                    //destroy object that collided or remove collider
                    if (Obstacle.DestroyOnPlayerCollision)
                    {
                        Destroy(collision.collider.gameObject);
                    }
                    else if (Obstacle.RemoveCollision)
                    {
                        collision.collider.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                else
                {
                    currentHealth -= 1;

                    //try and remove collider from tilemap
                    var TileMapCollid = collision.collider.GetComponent<TilemapCollider2D>();
                    if (TileMapCollid != null)
                    {
                        TileMapCollid.enabled = false;
                    }
                }

                // Game Over
                if (currentHealth <= 0)
                {
                    // Load score level
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
                }
                if (healthBarObj != null)
                {
                    healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
                    animationManager.SwitchTo(PlayerAnimationStates.Hurt);
                }
            }
            // Hit the floor
            if (collision.collider.gameObject.CompareTag("Floor"))
            {
                jumpsRemaining = MaxNumberOfJumps;
            }
        }
    }

    //check if on ground
    public bool IsGrounded()
    {
        return grounded;
    }

    //collider to see on the ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            grounded = true;
            //Debug.Log("Ground");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            grounded = true;
            //Debug.Log("Ground");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            grounded = false;
            //Debug.Log("Not Ground");
        }
    }
}
