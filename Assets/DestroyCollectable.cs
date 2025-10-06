using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollectable : MonoBehaviour
{
    public static void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
