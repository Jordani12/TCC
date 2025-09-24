using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IPlayerState
{
    Rigidbody rb;
    public void EnterState(PlayerStateManager player){
        rb = GetComponent<Rigidbody>();
    }
    public void UpdateState(PlayerStateManager player){
        if (player.isOnRamp)
            rb.velocity = Vector3.zero;
    }
    public void FixedUpdateState(PlayerStateManager player) { }
    public void ExitState(PlayerStateManager player) { }
}
