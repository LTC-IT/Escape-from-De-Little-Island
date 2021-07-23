//////////////////////////////////////////////////////////////
// CameraRelativeControl.cs
// Penelope iPhone Tutorial
//
// CameraRelativeControl creates a control scheme similar to what
// might be found in 3rd person platformer games found on consoles.
// The left stick is used to move the character, and the right
// stick is used to rotate the camera around the character.
// A quick double-tap on the right joystick will make the 
// character jump.
//////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

// This script must be attached to a GameObject that has a CharacterController
[RequireComponent (typeof(CharacterController))]
public class CameraRelativeControl : MonoBehaviour
{
    
    public Joystick moveJoystick;
    public Joystick rotateJoystick;
    public Transform cameraTransform;                           // The actual transform of the camera
    public Transform cameraPivot;                               // The transform used for camera rotation
    
    public float jumpSpeed = 8.0f;
    public float speed = 5.0f;                                  // Ground speed
    public float inAirMultiplier = 0.25f;                       // Limiter for ground speed while jumping
    
    public Vector2 rotationSpeed = new Vector2(50, 25);         // Camera rotation speed for each axis
    
    private Vector3 velocity;                                   // Used for continuing momentum while in air
    private Transform thisTransform;
    private CharacterController character;
    private AnimationController animationController;
    
    void FaceMovementDirection()
    {
        Vector3 horizontalVelocity = character.velocity;
        horizontalVelocity.y = 0; // Ignore vertical movement
        
        // If moving significantly in a new direction, point that character in that direction
        if (horizontalVelocity.magnitude > 0.1f)
            thisTransform.forward = horizontalVelocity.normalized;
    }
    
    void OnEndGame()
    {
        // Disable joystick when the game ends  
        moveJoystick.Disable();
        rotateJoystick.Disable();
        
        // Don't allow any more control changes when the game ends
        this.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        
        // Cache component lookup at startup instead of doing this every frame
        thisTransform = GetComponent<Transform>();
        character = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        
        // Set the maximum speed, so that the animation speed adjustment work
        animationController.maxForwardSpeed = speed;
        
        // Move the character to the correct start position in the level, if one exists
        GameObject spawn = GameObject.Find("PlayerSpawn");
        
        if (spawn)
            thisTransform.position = spawn.transform.position;
        
    
    }
    
    // Update is called once per frame
    void Update()
    {
        
        Vector3 movement = cameraTransform.TransformDirection(new Vector3(moveJoystick.position.x, 0.0f, moveJoystick.position.y));
        
        // We only want the camera-space horizontal direction
        movement.y = 0.0f;
        movement.Normalize();   // Adjust magnitude after ignoring vertical movement
        
        // Let's use the largest component of the joystick position for the speed.
        Vector2 absJoyPos = new Vector2(Mathf.Abs(moveJoystick.position.x), Mathf.Abs(moveJoystick.position.y));
        movement *= speed * ((absJoyPos.x > absJoyPos.y) ? absJoyPos.x : absJoyPos.y);
        
        // Check for jump
        if (character.isGrounded)
        {
            if (rotateJoystick.tapCount == 2)
            {
                // Apply the current movement to launch velocity
                velocity = character.velocity;
                velocity.y = jumpSpeed;
            }
        } 
        else
        {
            // Apply gravity to our velocity to diminish it over time
            velocity.y += Physics.gravity.y * Time.deltaTime;
            
            // Adjust additional movement while in-air
            movement.x *= inAirMultiplier;
            movement.z *= inAirMultiplier;
        }
        
        movement += velocity;
        movement += Physics.gravity;
        movement *= Time.deltaTime;
        
        
        // Actually move the character
        character.Move(movement);
        
        if (character.isGrounded)
            // Remove any persistent velocity after landing
            velocity = Vector3.zero;
        

        // Face the character to match with where she is moving
        FaceMovementDirection();
        
        // Scale joystick input with rotation speed
        Vector2 camRotation = rotateJoystick.position;
        camRotation.x *= rotationSpeed.x;
        camRotation.y *= rotationSpeed.y;
        camRotation *= Time.deltaTime;
        
        // Rotate around the character horizontally in world, but use local space
        // for vertical rotation
        cameraPivot.Rotate(0, camRotation.x, 0, Space.World);
        cameraPivot.Rotate(camRotation.y, 0, 0);
            
    
    }
    
}
