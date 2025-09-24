using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //settings
    private float radiusDistance = 5;       // Raio de detecção de colisores
    private float explosionForce = 400;   
    private float explosionRadius = 5;      // Raio de influência da força
    private WaitForSeconds tempoEspera = new WaitForSeconds(0.1f);
    public GameObject barrel;             

    private void Awake()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);     
    }

    void Start()
    {
        Explode();
        StartCoroutine(barrel_desapear());
    }

    private void Explode()
    {
        // Detecta todos os colisores dentro do raio
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusDistance);
        
        foreach (Collider hit in colliders)
        {
            give_damage(hit);
            aplicate_force(hit);
        }
    }

    private void aplicate_force(Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 2f);
        }
    }

    private void give_damage(Collider hit)
    {
        Life life = hit.GetComponent<Life>();
        if (life != null)
        {
            if (hit.transform.tag == "Player")
                life.TakeDamage(20);
            else
                life.TakeDamage(40);
        }
    }

    private IEnumerator barrel_desapear()
    {
        yield return tempoEspera;
        Destroy(barrel);     
    }
}