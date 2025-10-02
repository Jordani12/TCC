using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawEnemy : MonoBehaviour
{
    public List <GameObject> enemys = new List <GameObject>();

    private void Start()
    {
        foreach (GameObject obj in enemys)
        {
            obj.SetActive(false);
        }
    }

    public void Spawn()
    {
        foreach(GameObject gameObject in enemys)
        {
            gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }
}
