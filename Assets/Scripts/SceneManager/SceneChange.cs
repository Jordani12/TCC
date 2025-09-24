using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string nameScene;
    private bool canEntry;
    private void Update()
    {
        if (canEntry){
            Logic();
        }       
    }
    private void OnTriggerEnter(Collider other){//check se esta perto da porta
        if(other.gameObject.tag == "Player"){
            canEntry = true;
        }
    }
    void Logic()
    {
        if (this.nameScene != null) { SceneManager.LoadScene(this.nameScene); }
        canEntry = false;
    }
}
