using System.Collections;
using UnityEngine;

public class CactoDamage : MonoBehaviour
{
    [SerializeField] private int damageOnStay = 2;
    private Life lifeInContact;
    private float lastDamageTime;

    private void OnCollisionEnter(Collision collision)
    {
        lifeInContact = collision.gameObject.GetComponent<Life>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Life>() == lifeInContact)
        {
            lifeInContact = null;
        }
    }

    private void Update()
    {
        if (lifeInContact != null && Time.time - lastDamageTime >= 0.5f)
        {
            lifeInContact.TakeDamage(this.damageOnStay);
            lastDamageTime = Time.time;
        }
    }
}
