using System.Collections;
using UnityEngine;
public class JumpingState : MonoBehaviour, IPlayerState
{
    private AirPhysics physics;
    public void EnterState(PlayerStateManager player)
    {
        physics = GetComponent<AirPhysics>();
        if (physics == null) return; 
        physics.Jump();
    }
    public void FixedUpdateState(PlayerStateManager player) { }
    public void UpdateState(PlayerStateManager player) { }
    public void ExitState(PlayerStateManager player) { }
}