using UnityEngine;

public class Animation
{
    static public void Anim(Animator anim, string nameAnim) {
        if (anim != null)
            anim.Play(nameAnim, 0, 0f);
    }
}
