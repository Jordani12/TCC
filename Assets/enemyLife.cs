using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    [Header("Life")]
    public int actualLife;
    public int maxLife;
    [Header("Effect")]
    [SerializeField] private ParticleSystem shine_part;

    private Main_Animation anim;

    private void Start()
    {
        actualLife = maxLife;
        anim = FindObjectOfType<Main_Animation>();
        if (anim == null) Debug.Log("Null");
        if (actualLife >= maxLife / 4) shine_part.Stop();
    }

    public void TakeDamage(int value)
    {
        actualLife -= value;
        if (actualLife <= 0) {
            actualLife = 0;
            if (gameObject.transform.parent != null) {//caso o componente life n�o esteja no objeto que queira destruir em si
                Transform parent = gameObject.transform.parent;
                Die(parent.gameObject);
            }
            else
                Die(gameObject);
        }
        shines(can_finalizate());
        anim.X_anim();
    }

    private void Die(GameObject objeto) {
        Destroy(objeto);
    }

    private bool can_finalizate() {
        if(actualLife <= maxLife / 4) return true;
        return false;
    }

    private void shines(bool activate) {
        if(activate)
            shine_part.Play();
    }

}
