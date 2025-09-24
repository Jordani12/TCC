using System.Collections;
using UnityEngine;

public class TutorialController
{
    public static IEnumerator Activate(GameObject toActive) {
        toActive.SetActive(true);
        yield return new WaitForSeconds(2);
        toActive.SetActive(false);
    }
}
