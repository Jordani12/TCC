/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplingState : MonoBehaviour, IPlayerState
{
    GraplingHook graplingHook;
    public void EnterState(PlayerStateManager player)
    {
        player.onGrapling = true;
        graplingHook = FindObjectOfType<GraplingHook>();
        if(graplingHook != null){
            graplingHook.CheckCanGrab(true);
        }
    }

    public void UpdateState(PlayerStateManager player) {
        if (Input.GetKeyUp(KeyCode.G)) {
            player.onGrapling = false;
            graplingHook.mantem = false;
            if (graplingHook != null)
            {
                graplingHook.CheckCanGrab(false);
            }
            graplingHook.isGrappling = false;
        }
    }

    public void FixedUpdateState(PlayerStateManager player)
    {
        
    }
    public void ExitState(PlayerStateManager player) { }
}*/