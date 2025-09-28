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
    private bool gravityAppliedThisFrame = false;

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

        gravityAppliedThisFrame = false;
    }

    private void floorExit(){
        float offset = playerManager.isOnRamp ? 0.3f : 0.1f;
        transform.position += Vector3.up * offset;
    }

    private void FixedUpdate()
    {
        if (!_isGrounded && !gravityAppliedThisFrame)
        {
            GravityController();
            gravityAppliedThisFrame = true;
        }
        else if (_isGrounded)
        {
            gravityAppliedThisFrame = false;
        }
    }

    private void GravityController()
    {
        float gravityMultiplier;
        float currentYVelocity = rb.velocity.y;

        if (currentYVelocity > 0.1) // Subindo
            gravityMultiplier = upwardMultiplier;
        else if (currentYVelocity < -0.1) // Descendo
            gravityMultiplier = downwardMultiplier;
        else // Neutro
            gravityMultiplier = defaultGravityScale;

        //apply custom gravity
        Vector3 gravity = Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        rb.velocity += gravity;
    }
}
