using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour
{
    private WaitForSeconds animationTime = new WaitForSeconds(1f);
    public GameObject explosion;
    public AudioSource explosionSource;
    public GameObject explosionEffect;
    public Transform inspector;
    public IEnumerator Explosion()
    {
        //starta anima��o
        yield return animationTime;

        if (explosionSource != null)
            explosionSource.Play();

        if (explosionEffect != null)
            Instantiate(explosionEffect, inspector); 
        
        if (explosion != null)
            explosion.SetActive(true);    
    }
}
