using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private GunController gunController_scr;

    //rotations
    private Vector3 targetRotation;

    //bools
    private bool isAiming;

    //fire recoil
    [SerializeField] private Vector3 RecoilRotation = new Vector3(-2f, 1f, 0f);

    //aim recoil
    [SerializeField] private Vector3 AimRecoilRotation = new Vector3(-1.5f, 0.5f, 0f);

    //settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private Vector3 currentRecoilRotation;
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRecoilRotation = Vector3.Slerp(currentRecoilRotation, targetRotation, snappiness * Time.deltaTime);

        if (gunController_scr == null) return;

        isAiming = GunController.isAiming;
    }

    public Quaternion GetCurrentRecoilRotation()
    {
        if (gunController_scr.currentGun.getGun.gunName == "Weapon") return Quaternion.identity;
        else if(gunController_scr.currentGun.getGun.gunName == "Shotgun") return Quaternion.Euler(currentRecoilRotation * 3);

        return Quaternion.Euler(currentRecoilRotation);
    }

    public void RecoilFire()
    {
        if (isAiming)
            targetRotation += new Vector3(
                AimRecoilRotation.x, 
                Random.Range(-AimRecoilRotation.y, AimRecoilRotation.y), 
                Random.Range(-AimRecoilRotation.z, AimRecoilRotation.z));
        else
            targetRotation += new Vector3(
                RecoilRotation.x, 
                Random.Range(-RecoilRotation.y, RecoilRotation.y), 
                Random.Range(-RecoilRotation.z, RecoilRotation.z));

        targetRotation = Vector3.ClampMagnitude(targetRotation, 30f);
    }
}
