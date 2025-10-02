using UnityEngine;

public class OnEnterSpawnEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "spawnEnemy"){
            SpawEnemy spawnEn = other.GetComponent<SpawEnemy>();
            if(spawnEn != null) {spawnEn.Spawn();}
        }
    }
}
