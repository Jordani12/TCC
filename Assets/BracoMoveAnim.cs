using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BracoMoveAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private PlayerStateManager player;

    private void Start()
    {
        player = FindObjectOfType<PlayerStateManager>();
        Move_Animation(true);
    }

    private void Update()
    {
        CheckEvent();
    }

    public void CheckEvent()
    {
        if(player.isMoving) {
            Move_Animation(false);
            return;
        }

        Move_Animation(true);
    }

    private void Move_Animation(bool isIdle)
    {
        if (isIdle) {
            anim.SetTrigger("idle");
            return;
        }

        anim.SetTrigger("move");
    }
}
