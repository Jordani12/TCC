using System.Collections;
using UnityEngine;

public class WalkingState : MonoBehaviour, IPlayerState
{
    private PlayerStateManager _player;
    private Rigidbody rb;
    
    public void EnterState(PlayerStateManager player){
        rb = GetComponent<Rigidbody>();

        _player = player;
        if (_player == null) return;
        _player.isMoving = true;
    }

    private float speed => _player.walkSpeed;
    private float maxSpeed => _player.MaxWalkSpeed;
    private float drag => _player.Drag;

    public void UpdateState(PlayerStateManager player){
        HandleDrag(player);
    }
    public void FixedUpdateState(PlayerStateManager player)
    {
        HandleMove(player);
        LimitVelocity(player);
    }

    public void ExitState(PlayerStateManager player) { _player.isMoving = false; }

    private void HandleMove(PlayerStateManager player)
    {
        Vector3 combinedMove = Vector3.zero;

        combinedMove = player.GetMovementInput();

        // Normaliza o vetor para manter velocidade constante em todas as direções
        if (combinedMove != Vector3.zero && player.canMove)
        {
            //combinedMove = combinedMove.normalized;
            MoveDir(combinedMove, player);
        }
    }

    private void HandleDrag(PlayerStateManager player)
    {
        if (player.isGrounded)
        {
            // Aplica drag apenas no eixo horizontal quando no chão
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            horizontalVelocity /= (1 + drag * Time.deltaTime);
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
        else
        {
            // Menos drag no ar para controle mais suave
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            horizontalVelocity /= (1 + (drag * 0.1f) * Time.deltaTime);
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
        /*if (!player.isGrounded)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z) / (1 + drag / 100) + new Vector3(0, rb.velocity.y, 0);
        else
            rb.velocity /= (1 + drag / 100);*/
    }

    private void LimitVelocity(PlayerStateManager player)
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
        /*if (!player.isGrounded)
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (horizontalVelocity.magnitude > maxSpeed)
            {
                Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z).normalized;
            }
        }
        else
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                Vector3 limitedVelocity = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }*/
    }

    private void MoveDir(Vector3 moveDir, PlayerStateManager player)
    {
        Quaternion cameraRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        Vector3 cameraOrientedDir = cameraRotation * moveDir;

        Vector3 forceDirection = cameraOrientedDir;

        // Se estiver no chão, projeta na superfície
        if (player.isGrounded)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, out RaycastHit hit, 0.3f))
            {
                forceDirection = Vector3.ProjectOnPlane(cameraOrientedDir, hit.normal).normalized;
            }
        }

        float currentSpeed = speed;

        // Reduz força no ar
        if (!player.isGrounded)
        {
            currentSpeed = speed * 0.7f; // 10% da força no ar
        }

        Vector3 force = forceDirection * currentSpeed;
        rb.AddForce(force, ForceMode.Force);
        /*Quaternion dir = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        Vector3 planeNormal = Vector3.up;
        if(!player.isGrounded)
            if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                planeNormal = hit.normal;
        
        Vector3 force = Vector3.ProjectOnPlane(dir * moveDir, planeNormal) * speed;
        rb.AddForce(force);*/
    }
}
