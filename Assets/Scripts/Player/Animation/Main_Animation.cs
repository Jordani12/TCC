using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Main_Animation : MonoBehaviour
{
    public Animator cam_animator;
    public Animator killed_anim;
    public Animator damage_anim;
    public Animator _change_gun_anim;
    private enemyLife enLife;

    private void Start()
    {
        enLife = FindObjectOfType<enemyLife>();
    }

    public void DeadMoveCam()
    {
        MenuPause.can_change_canvas = false;
        cam_animator.SetBool("died", true);
    }

    public void X_anim()
    {
        if (killed_anim == null || damage_anim == null) return;

        if (enLife.actualLife <= 0)
        {
            killed_anim.SetBool("activate_fade", false);
            damage_anim.SetBool("activate_fade", false);
            killed_anim.SetBool("activate_fade", true);
            StartCoroutine(animation_reset(killed_anim));
        }
        else
        {
            damage_anim.SetBool("activate_fade", false);
            killed_anim.SetBool("activate_fade", false);
            damage_anim.SetBool("activate_fade", true);

            StartCoroutine(animation_reset(damage_anim));
        }

    }

    private IEnumerator animation_reset(Animator anim)
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("activate_fade", false);
    }

    public void Change_Gun_Anim(){
        _change_gun_anim.SetTrigger("change");
    }
}

