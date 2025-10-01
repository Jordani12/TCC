using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestructible : MonoBehaviour
{
    //componentes

    //door's settings
    public List<GameObject> woods = new List<GameObject>();
    private int count;

    private void Update()
    {
        TakeDamage();
    }

    public void CountDamage()
    {
        count++;
    }

    private void TakeDamage()
    {
        if (count == 2)
        {
            count = 0; 
            DestroyWood(); 
        }

        if (woods.Count == 0)
        {
            BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
            collider.isTrigger = true; 
            this.enabled = false; 
        }
    }

    private void DestroyWood()
    {
        // Verifica se ainda existe madeira para destruir
        if (woods[woods.Count - 1] != null)
        {
            // Destroi a última madeira da lista
            Destroy(woods[woods.Count - 1]);
            // Remove a referência da lista
            woods.Remove(woods[woods.Count - 1]);
        }
    }
}