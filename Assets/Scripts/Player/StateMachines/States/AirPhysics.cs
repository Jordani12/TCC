using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPhysics : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStateManager playerManager;

    [Header("Jump Settings")]
    [SerializeField, Range(0f, 2000f)] private float jumpForce = 500f;

    [Header("Gravity Multipliers")]
    [SerializeField, Range(0f, 5f)] private float downwardMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMultiplier = 1.7f;

    private float defaultGravityScale = 1f;

    [HideInInspector] public bool isJumping;
    private void Awake()
    {
        playerManager = GetComponent<PlayerStateManager>();
        rb = GetComponent<Rigidbody>();
    }
    private bool _isGrounded => playerManager.isGrounded;

    public void Jump(){
        if (!playerManager.isGrounded) return;

        floorExit();

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void floorExit(){
        if (playerManager.isOnRamp)
        {
            transform.position += Vector3.up * .3f;
        }
            
        else
            transform.position += Vector3.up * .1f;
    }

    private void FixedUpdate()
    {
        if (!_isGrounded)
        {
            GravityController();
        }
    }

    private void GravityController()
    {
        float gravityMultiplier;

        if (rb.velocity.y > 0) // Subindo
            gravityMultiplier = upwardMultiplier;
        else if (rb.velocity.y < 0) // Descendo
            gravityMultiplier = downwardMultiplier;
        else // Neutro
            gravityMultiplier = defaultGravityScale;

        //apply custom gravity
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }
}
