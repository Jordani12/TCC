using UnityEngine;
public class CollectibleController : MonoBehaviour
{
    private float rotX, rotY;
    public float sensivity = 200;
    private void Update(){
        RotateObject();
    }
    private void RotateObject(){
        rotX = Input.GetAxis("Mouse X"); rotY = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(rotY, rotX, 0) * Time.deltaTime *  sensivity);
    }
}
