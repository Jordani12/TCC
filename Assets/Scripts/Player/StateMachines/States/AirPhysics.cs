using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AirPhysics : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStateManager playerManager;

    [Header("Jump Settings")]
    [SerializeField, Range(0f, 20)] private float jumpForce = 10f;

    [Header("Gravity Multipliers")]
    [SerializeField, Range(0f, 5f)] private float downwardMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMultiplier = 1.7f;

    private float defaultGravityScale = 1f;

    [HideInInspector] public bool isJumping;

    private float jumpCooldown = 0.2f;
    private float lastJumpTime = 0f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerStateManager>();
        rb = GetComponent<Rigidbody>();
    }
    private bool _isGrounded => playerManager.isGrounded;

    public void Jump(){
        if (Time.time - lastJumpTime < jumpCooldown) return;

        lastJumpTime = Time.time;
        isJumping = true;
        //floorExit();

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(playerManager.release_checking_ground());
    }

    private void floorExit(){
        float offset = playerManager.isOnRamp ? 0.3f : 0.1f;
        transform.position += Vector3.up * offset;
    }

    private void FixedUpdate()
    {
        if(!_isGrounded) GravityController();
        else isJumping = false;
        
    }

    private void GravityController()
    {
        float gravityMultiplier;
        float currentYVelocity = rb.velocity.y;

        if (currentYVelocity > 0 ) // Subindo
            gravityMultiplier = upwardMultiplier;
        else if (currentYVelocity < 0) // Descendo
            gravityMultiplier = downwardMultiplier;
        else // Neutro
            gravityMultiplier = defaultGravityScale;

        Vector3 gravity = Physics.gravity.y * gravityMultiplier * Vector3.up * Time.fixedDeltaTime;
        rb.velocity += gravity;
        //rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }    
}
