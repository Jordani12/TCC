using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    private float targetFOV = 40;
    private float defaultFOV = 70;
    private float zoomSpeed = 8f;

    [HideInInspector] public static bool canChangeMove = false;

    private Vector3 normalWeaponPosition = new Vector3(0.34f, -0.1f, 0.5f);
    private Vector3 aimingWeaponPosition = new Vector3(0f, -0.1f, 0.5f);
    private float weaponMoveSpeed = 8f;

    //coroutines
    private Coroutine zoomCoroutine;
    [SerializeField] private SO_SaveInputs inputs;

    private GunController gun;
    
    private void Start()
    {
       gun = GetComponent<GunController>();
    }

    private void Update()
    {
        targetFOV = inputs.fov - 30;
        defaultFOV = inputs.fov;
    }

    public void start_aim_increase()
    {
        GunController.isAiming = true;
        if (zoomCoroutine != null) { StopCoroutine(zoomCoroutine); }

        zoomCoroutine = StartCoroutine(GunController.aim_Increase(true, targetFOV, defaultFOV, zoomSpeed));
    }

    public void releasing_aim()
    {
        canChangeMove = true;
        GunController.isAiming = false;
        if (zoomCoroutine != null) { StopCoroutine(zoomCoroutine); }
        zoomCoroutine = StartCoroutine(GunController.aim_Increase(false, targetFOV, defaultFOV, zoomSpeed));
    }

    public void aim_moving_weapon(PlayerStateManager player)
    {
        if (canChangeMove) { speedOnZoom(true, player); }
        moveWeapon(aimingWeaponPosition);
    }

    public void aim_release_weapon(PlayerStateManager player)
    {
        if (canChangeMove) { speedOnZoom(false, player); }
        moveWeapon(normalWeaponPosition);
    }

    private void speedOnZoom(bool zoom, PlayerStateManager player)
    {
        canChangeMove = false;
        float lessOrPlus = zoom ? 0.5f : 2f;
        player.walkSpeed *= lessOrPlus; return;
    }

    private void moveWeapon(Vector3 targetPosition)
    {
        if (gun == null) return;
        gun.currentGun.model.transform.localPosition = Vector3.Lerp(gun.currentGun.model.transform.localPosition, targetPosition, weaponMoveSpeed * Time.deltaTime);
    }
}
