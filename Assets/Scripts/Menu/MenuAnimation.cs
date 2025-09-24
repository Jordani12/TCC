using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    public Animator anim;
    public void FadeInMenuOptions()
    {
        anim.SetBool("StartFade", true);
    }
}
