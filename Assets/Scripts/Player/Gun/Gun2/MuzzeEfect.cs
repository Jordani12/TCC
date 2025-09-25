using System.Collections;
using UnityEngine;
public class MuzzeEfect : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyObject());
    }
    private IEnumerator DestroyObject()//só para não manter o efeito ativo
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
    }
}
