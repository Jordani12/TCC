using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public enum PlayerState { Idle, Dash, Walk, Jump, Finalizate }

public class PlayerStateManager : MonoBehaviour
{
    //estado atual do jogador (usando interface IPlayerState)
    private Rigidbody rb;

    private IPlayerState[] states;
    private PlayerState currentStateType;

    public SO_SaveInputs inputs_SO;

    [Header("Layers")]
    public LayerMask Ground;
    public LayerMask Ramp;

    [Header("Configurações de Movimento")]
    public Transform orientation;
    public float walkSpeed = 5f, MaxWalkSpeed = 3f, Drag = 4f;
    public float dashForce = 10f;

    [Header("Slider")]
    public Slider dashSlider;

    public bool isDashing { get; set; } = false;
    public bool canJump { get; set; } = true;
    public bool isMoving { get; set; } = false;
    public bool isGrounded { get; private set; }
    public bool canDash { get; set; } = true;
    public bool canMove { get; set; } = true;
    public bool isOnRamp { get; private set; }
    public bool IsMoving { get; private set; }

    //inputs properties
    public KeyCode forward_in => inputs_SO.forward_in;
    public KeyCode backward_in => inputs_SO.backward_in;
    public KeyCode left_in => inputs_SO.left_in;
    public KeyCode right_in => inputs_SO.right_in;
    public KeyCode dash_in => inputs_SO.dash_in;
    public KeyCode finalization_in => inputs_SO.finalization_in;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitializateState();
    }

    void Start()
    { 
        //valores iniciais padrão
        canMove = true; 
        canDash = true;
    }

    private void InitializateState()
    {
        states = new IPlayerState[5];
        states[(int)PlayerState.Walk] = gameObject.AddComponent<WalkingState>();
        states[(int)PlayerState.Idle] = gameObject.AddComponent<IdleState>();
        states[(int)PlayerState.Dash] = gameObject.AddComponent<DashState>();
        states[(int)PlayerState.Jump] = gameObject.AddComponent<JumpingState>();
        states[(int)PlayerState.Finalizate] = gameObject.AddComponent<WalkingState>();

        foreach (var state in states)
        {
            if (state is MonoBehaviour behaviour) behaviour.enabled = false;
        }

        SwitchState(PlayerState.Idle);
    }

    void Update()
    {
        UpdateState();
        HandleStateTransition();
    }

    private void FixedUpdate()
    {
        CheckGround();
        FixedUpdateState();
    }

    private void FixedUpdateState()
    {
        GetCurrentState().FixedUpdateState(this);
    }

    private void UpdateState()
    {
        GetCurrentState().UpdateState(this); 
    }

    public void SwitchState(PlayerState newState)
    {
        GetCurrentState().ExitState(this);
        currentStateType = newState;
        GetCurrentState().EnterState(this);
    }

    public bool IsInState(PlayerState state)
    {
        return currentStateType == state;
    }

    private IPlayerState GetCurrentState() => states[(int)currentStateType];

    public void CheckGround()
    {
        ramp_ground_Verification();

        changeGravity();

        canJump = isGrounded && !isDashing;
    }

    private void ramp_ground_Verification()
    {
        float checkDistance = 1.3f;
        float sphereRadius = 0.4f;

        bool groundHit = Physics.SphereCast(transform.position, sphereRadius,
            Vector3.down, out RaycastHit groundHitInfo, checkDistance, Ground);

        bool rampHit = Physics.SphereCast(transform.position, sphereRadius,
            Vector3.down, out RaycastHit rampHitInfo, checkDistance, Ramp);

        isGrounded = groundHit || rampHit;
        isOnRamp = rampHit && !groundHit;
        /*isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, Ground);
        isOnRamp = isGrounded && Physics.Raycast(transform.position, Vector3.down, 1.5f, Ramp);*/
    }

    private void OnDrawGizmos()
    {
        // Visualizar raycast/spherecast de ground detection
        
        if (isOnRamp)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.65f, 0.4f);
        }
        else if (isGrounded)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector3.down * 1.3f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * 1.3f);
        }
        
    }

    private void changeGravity()
    {
        if (isGrounded)
            rb.useGravity = false;
        else
            rb.useGravity = true;
    }

    public Vector3 GetMovementInput()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(forward_in)) input.z += 1;
        if (Input.GetKey(backward_in)) input.z -= 1;
        if (Input.GetKey(right_in)) input.x += 1;
        if (Input.GetKey(left_in)) input.x -= 1;
        return input.normalized;
    }

    private void HandleStateTransition()
    {
        bool move = Input.GetKey(forward_in) || Input.GetKey(left_in)
                 || Input.GetKey(backward_in) || Input.GetKey(right_in);
        
        if (IsInState(PlayerState.Finalizate)) return;

        //finalization
        if (Input.GetKeyDown(finalization_in))
        {
            SwitchState(PlayerState.Finalizate);
        }
        //jump
        else if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            SwitchState(PlayerState.Jump);
        }
        //dash
        else if (Input.GetKeyDown(dash_in) && canDash)
        {
            isDashing = true; // Ativa flag
            SwitchState(PlayerState.Dash);
        }
        //move
        if (!IsInState(PlayerState.Dash))
        {
            if (move != false && canMove)
            {
                SwitchState(PlayerState.Walk);
            }
            else
            {
                SwitchState(PlayerState.Idle);
            }
        }
        
    }

}