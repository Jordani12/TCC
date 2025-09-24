using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementGun : MonoBehaviour
{
    private GunController _gun;
    private void Start()
    {
        _gun = GetComponent<GunController>();
    }
    public void replacement_shotgun()
    {
        foreach (var gun in _gun.inventory)
            if (gun.canEquipShotgun) {
                _gun.currentGun.getGun.isMelee = false; StartCoroutine(_gun.Change(1));
            }
    }

    public void replacement_melee()
    {
        _gun.currentGun.getGun.isMelee = true; StartCoroutine(_gun.Change(2));
    }

    public void replacement_pistol()
    {
        _gun.currentGun.getGun.isMelee = false; StartCoroutine(_gun.Change(0));
    }
}
