using System.Collections;
using UnityEngine;
public class MuzzeEfect : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyObject());
    }
    private IEnumerator DestroyObject()//s� para n�o manter o efeito ativo
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
    }
}
