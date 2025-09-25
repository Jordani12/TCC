using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionHandle : MonoBehaviour
{
    [SerializeField] private HeadCollisionDetector detector;
    [SerializeField] private GameObject player;
    private Rigidbody playerRB;
    [SerializeField] private float pushBackStrength = 1f;

    private void Start()
    {
        if (player != null)
            playerRB = player.GetComponent<Rigidbody>();
    }

    private Vector3 CalculatePushBackDirection(List<RaycastHit> colliderHits)
    {
        Vector3 combinedNormal = Vector3.zero;
        foreach(RaycastHit hitPoint in colliderHits)
            combinedNormal += new Vector3(hitPoint.normal.x, 0, hitPoint.normal.z);

        return combinedNormal;
    }

    private void Update()
    {
        if (detector.DetectedColliderHits.Count <= 0)
            return;
        Vector3 pushBackDirection = CalculatePushBackDirection(detector.DetectedColliderHits);

        playerRB.AddForce(pushBackDirection.normalized * pushBackStrength * Time.deltaTime);
    }
}
