using UnityEngine;

public class Generic : MonoBehaviour
{
    public static GameObject GetClosestObjects(Transform reference, GameObject[] references)
    {
        GameObject tMin = null; //armazena o objeto mais próximo encontrado
        float minDist = Mathf.Infinity;
        Vector3 currentPos = reference.position;

        foreach (GameObject t in references)
        {
            float dis = Vector3.Distance(t.transform.position, currentPos);
            
            //se a distância for menor que a menor distância registrada até agora
            if (dis < minDist)
            {
                tMin = t; // Atualiza o objeto mais próximo
                minDist = dis; // Atualiza a menor distância
            }
        }

        return tMin;
    }
}