using System.Collections;
using UnityEngine;

public class DestroyMainBarrel : MonoBehaviour
{
    public GameObject mainBarrel;
    public bool canCheck;
    private void Start(){
        canCheck = true;
    }

    private void Update(){
        if (transform.childCount != 0 && canCheck) {StartCoroutine(WaitForEffects()); }
    }

    private IEnumerator WaitForEffects(){   
        canCheck = false;
        yield return new WaitForSeconds(3f);
        Destroy(mainBarrel);
    }
}
