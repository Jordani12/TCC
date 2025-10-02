using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_test : MonoBehaviour
{

    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

}
