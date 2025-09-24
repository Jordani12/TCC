using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class DashState : MonoBehaviour, IPlayerState
{
    private const float dashDuration = 0.4f;

    private const float delay = 0.15f;

    private Rigidbody rb;
    private DashCooldown dashCooldown_scr;
    private PlayerCamera cameraController;
    private PlayerStateManager player;
    public void EnterState(PlayerStateManager player){
        this.player = player;
        rb = GetComponent<Rigidbody>();
        dashCooldown_scr = GetComponent<DashCooldown>();
        cameraController = Camera.main.GetComponent<PlayerCamera>();
        StartCoroutine(Dash(speedDash, speedWalk, this.player, delay));
    }
    private float speedDash => player.dashForce;
    private float speedWalk => player.walkSpeed;

    private IEnumerator Dash(float speed, float speedWalk, PlayerStateManager player, float delayedTime)
    {
        player.canDash = false;
        yield return new WaitForSeconds(delayedTime);

        Quaternion world_dir = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        Vector3 input_dir = player.GetMovementInput();
        Vector3 dash_direction = world_dir * input_dir;

        if(cameraController != null) StartCoroutine(cameraController.ApplyDashEffect(dashDuration));

        float elapsed = 0;
        while (elapsed < dashDuration)
        {
            rb.velocity = dash_direction * player.dashForce;
            elapsed += Time.deltaTime;
            yield return null;
        }

        ResetDash(player);
    }

    private void ResetDash(PlayerStateManager player){
        rb.velocity = Vector3.zero;
        player.isDashing = false;
        player.canJump = true;
        player.SwitchState(PlayerState.Idle);
        CallCooldown();
    }

    private void CallCooldown()
    {
        dashCooldown_scr.onCooldown = true;
    }

    public void UpdateState(PlayerStateManager player) { }
    public void FixedUpdateState(PlayerStateManager player) { }
    public void ExitState(PlayerStateManager player) { }
}