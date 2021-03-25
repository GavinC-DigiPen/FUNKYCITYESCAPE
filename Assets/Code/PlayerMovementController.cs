//------------------------------------------------------------------------------
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

public class PlayerMovementController : MonoBehaviour
{
    [Tooltip("The health bar feedback bar")]
    public GameObject healthBarObj;
    [Tooltip("The distance text mesh pro object")]
    public GameObject distanceObj;
    [Tooltip("The autorun speed of the player, used to track distance traveled")]
    public float MoveSpeed = 10;
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
    [Tooltip("The key that will be used for jumping")]
    public KeyCode JumpKey = KeyCode.Space;
    [Tooltip("The key that will be used for sliding")]
    public KeyCode Slide_SlamKey = KeyCode.S;
    [Tooltip("The key that will be used for moving right")]
    public KeyCode RightKey = KeyCode.D;
    [Tooltip("The key that will be used for moving left")]
    public KeyCode LeftKey = KeyCode.A;
    [Tooltip("The box collider offset while sliding")]
    public Vector2 SlidingColliderOffset;
    [Tooltip("The box collider size while sliding")]
    public Vector2 SlidingColliderSize;

    int jumpsRemaining = 0;
    int currentHealth = 0;
    float startingX = 0;
    PlayerAnimationManager animationManager;
    BoxCollider2D PlayerBoxCollider;
    Rigidbody2D PlayerRB;
    Vector2 StartingColliderOffset;
    Vector2 StartingColliderSize;
    float Dirrection = 0;

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
        startingX = transform.position.x;
        PlayerSaveData.DistanceRun = 0;
        JumpHeight = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * JumpHeight); // Take the square root of the jump height so that the math for gravity works to make the number the user enters the number of units the player will actually be able to jump
        StartingColliderOffset = PlayerBoxCollider.offset;
        StartingColliderSize = PlayerBoxCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        //check ground
        bool grounded = IsGrounded();

        //reset player hitbox
        PlayerBoxCollider.offset = StartingColliderOffset;
        PlayerBoxCollider.size = StartingColliderSize;

        // Jumping
        if (Input.GetKeyDown(JumpKey) && !Input.GetKey(Slide_SlamKey))
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
        else if (Input.GetKey(Slide_SlamKey) && !grounded)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Slam);
            var slam_vec = new Vector3(transform.position.x, -SlamSpeed, 0);
            PlayerRB.velocity = slam_vec;
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
        else if (!Input.GetKey(Slide_SlamKey) && grounded)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Run);
        }
        // Falling
        else
        {
            animationManager.SwitchTo(PlayerAnimationStates.Jump);
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
        PlayerSaveData.DistanceRun += MoveSpeed * Time.deltaTime;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hit an Obstacle
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                currentHealth -= obstacle.Damage;
                // Game Over
                if (currentHealth <= 0)
                {
                    // Load score level
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
                }
                if (obstacle.DestroyOnPlayerCollision)
                {
                    Destroy(collision.collider.gameObject);
                }
                if (healthBarObj != null)
                {
                    healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
                    animationManager.SwitchTo(PlayerAnimationStates.Hurt);
                }
            }
        }
        // Hit the floor
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            jumpsRemaining = MaxNumberOfJumps;
        } 
    }

    public bool IsGrounded()
    {
        return jumpsRemaining == MaxNumberOfJumps;
    }
}
