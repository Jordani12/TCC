using UnityEngine;
public class Follower : MonoBehaviour
{
    [SerializeField] private Transform follower;
    void Update(){
        follower.position = transform.position;
    }
}
