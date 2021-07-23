//////////////////////////////////////////////////////////////
// PlayerRelativeControl.cs
// Penelope iPhone Tutorial
//
///////////////////////////////////////////
using UnityEngine;
using System.Collections;

// This script must be attached to a GameObject that has a CharacterController
[RequireComponent (typeof(CharacterController))]
public class PlayerRelativeControl : MonoBehaviour
{
    
    public Joystick moveJoystick;
    public Joystick rotateJoystick;
    
    //public Transform cameraTransform;                         // The actual transform of the camera
    public Transform cameraPivot;                               // The transform used for camera rotation
    
    public float forwardSpeed = 6.0f;
    public float backwardSpeed = 3.0f;
    public float sidestepSpeed = 4.0f;
    public float jumpSpeed = 16.0f;
    public float speed = 6.0f;                                  // Ground speed
    public float inAirMultiplier = 0.25f;                       // Limiter for ground speed while jumping
    
    public Vector2 rotationSpeed = new Vector2(50, 25);     // Camera rotation speed for each axis
    
    private Vector3 velocity;                                   // Used for continuing momentum while in air
    private Vector3 cameraVelocity;
    private Transform thisTransform;
    private CharacterController character;
    private AnimationController animationController;
    
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
        animationController.maxForwardSpeed = forwardSpeed;
        animationController.maxBackwardSpeed = backwardSpeed;
        animationController.maxSidestepSpeed = sidestepSpeed;
        
        GameObject spawn = GameObject.Find("PlayerSpawn");
        
        if (spawn)
            thisTransform.position = spawn.transform.position;
    
    }
    
    // Update is called once per frame
    void Update()
    {
        
        Vector3 movement = thisTransform.TransformDirection(new Vector3(moveJoystick.position.x, 0.0f, moveJoystick.position.y));
        
        // We only want the camera-space horizontal direction
        movement.y = 0.0f;
        movement.Normalize();   // Adjust magnitude after ignoring vertical movement
        
        // Let's use the largest component of the joystick position for the speed.
        Vector3 cameraTarget = Vector3.zero;
        Vector2 absJoyPos = new Vector2(Mathf.Abs(moveJoystick.position.x), Mathf.Abs(moveJoystick.position.y));
        
        if (absJoyPos.y > absJoyPos.x)
        {
            if (moveJoystick.position.y > 0)
                movement *= forwardSpeed * absJoyPos.y;
            else
            {
                movement *= backwardSpeed * absJoyPos.y;
                cameraTarget.z = moveJoystick.position.y * 0.75f;
            }
        } else
        {
            movement *= sidestepSpeed * absJoyPos.x;
            cameraTarget.x = -moveJoystick.position.x * 0.5f;
        }
        
        
        
        // Check for jump
        if (character.isGrounded)
        {
            if (rotateJoystick.tapCount == 2)
            {
                // Apply the current movement to launch velocity
                velocity = character.velocity;
                velocity.y = jumpSpeed;
            }
        } else
        {
            // Apply gravity to our velocity to diminish it over time
            velocity.y += Physics.gravity.y * Time.deltaTime;
            
            cameraTarget.z = -jumpSpeed * 0.25f;
            
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
        
        Vector3 pos = cameraPivot.localPosition;
        
        pos.x = Mathf.SmoothDamp(pos.x, cameraTarget.x, ref cameraVelocity.x, 0.3f);
        pos.z = Mathf.SmoothDamp(pos.z, cameraTarget.z, ref cameraVelocity.x, 0.5f);
        
        cameraPivot.localPosition = pos;
        
        if (character.isGrounded)
        {
        
            // Scale joystick input with rotation speed
            Vector2 camRotation = rotateJoystick.position;
            camRotation.x *= rotationSpeed.x;
            camRotation.y *= rotationSpeed.y;
            camRotation *= Time.deltaTime;
            
            // Rotate around the character horizontally in world, but use local space
            // for vertical rotation
            thisTransform.Rotate(0, camRotation.x, 0, Space.World);
            cameraPivot.Rotate(camRotation.y, 0, 0);
        
        }
    }
    
}
