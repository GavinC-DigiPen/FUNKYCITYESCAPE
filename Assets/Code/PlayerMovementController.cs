//------------------------------------------------------------------------------
//
// File Name:	PlayerMovementController.cs
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
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementController : MonoBehaviour
{
    [Tooltip("The health bar feedback bar")]
    public GameObject healthBarObj;
    [Tooltip("The distance text mesh pro object")]
    public GameObject distanceObj;
    [Tooltip("The max and starting health")]
    public int MaxHealth = 3;
    [Tooltip("The speed of the player's sprint")]
    public float MovementSpeed = 5;
    [Tooltip("The jump force of the player")]
    public float JumpForce = 5;
    [Tooltip("The amount of time you can hold down space to jump higher.")]
    public float JumpTime = 0.2f;
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
    bool IsJumping = false;
    float JumpTimeCounter;
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

        //Stops higher jump when space is no longer being pressed
        if (Input.GetKeyUp(JumpKey))
        { 
            IsJumping = false;
        }

        // Jumping
        if (Input.GetKeyDown(JumpKey))
        {
            if (jumpsRemaining > 0)
            {
                animationManager.SwitchTo(PlayerAnimationStates.Jump);
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Vector2.up.y * JumpForce);
                jumpsRemaining -= 1;
                IsJumping = true;
                JumpTimeCounter = 0;
            }
        }
        //jump more if player is holding space
        else if (Input.GetKey(JumpKey) && IsJumping)
        {
            //Lets you keep jumping if time hasn't run out
            if (JumpTimeCounter < JumpTime)
            {
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Vector2.up.y * JumpForce);
                JumpTimeCounter += Time.deltaTime;
            }
            else
            {
                IsJumping = false;
            }
        }
        //Slam
        else if (Input.GetKey(Slide_SlamKey) && !grounded && animationManager.CurrentState != PlayerAnimationStates.Slide)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Slam);
            var slam_vec = new Vector3(transform.position.x, -SlamSpeed, 0);
            PlayerRB.velocity = slam_vec;
        }
        // Falling
        else if (!grounded && animationManager.CurrentState != PlayerAnimationStates.Slide && Input.GetKey(Slide_SlamKey))
        {
            animationManager.SwitchTo(PlayerAnimationStates.Jump);
        }
        // Sliding
        else if (Input.GetKey(Slide_SlamKey) && grounded && !IsJumping && animationManager.CurrentState != PlayerAnimationStates.Jump && animationManager.CurrentState != PlayerAnimationStates.Slam)
        { 
            animationManager.SwitchTo(PlayerAnimationStates.Slide);

            //change character hitbox
            PlayerBoxCollider.offset = SlidingColliderOffset;
            PlayerBoxCollider.size = SlidingColliderSize;
        }
        // Running
        else if (grounded)
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
        //give character hang time during animation
        if (AttackCooldownTimer > AttackCooldown - animationManager.TimeInAttack)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
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
        // Hit an Obstacle
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            ObstacleInfo Obstacle = collision.gameObject.GetComponent<ObstacleInfo>();
            var InvulnTime = GetComponent<PlayerAnimationManager>().CurrInvulnTime;

            //deal with things that have obstacle script
            if (Obstacle != null)
            {
                if (InvulnTime <= 0 && animationManager.AttackCooldownTimer < 0)
                {
                    currentHealth -= Obstacle.Damage;
                }

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
            //just do damage
            else
            {
                if (InvulnTime <= 0 && animationManager.AttackCooldownTimer < 0)
                {
                    currentHealth -= 1;
                }
            }

            //try and remove collider from tilemap
            var TileMapCollid = collision.collider.GetComponent<TilemapCollider2D>();
            if (TileMapCollid != null)
            {
                TileMapCollid.enabled = false;
            }

            //animation 
            if (animationManager.AttackCooldownTimer < 0)
            {
                animationManager.SwitchTo(PlayerAnimationStates.Hurt);
            }

            //health bar
            if (healthBarObj != null)
            {
                healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
            }

            // Game Over
            if (currentHealth <= 0)
            {
                // Load score level
                UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
            }
        }
        // Hit the floor
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            jumpsRemaining = MaxNumberOfJumps;
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
