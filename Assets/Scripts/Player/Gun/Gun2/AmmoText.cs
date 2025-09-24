using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    /*private void out_ammo_text(GameObject textLowAmmo, GameObject textNoAmmo)
    {
        textLowAmmo.SetActive(false);
        textNoAmmo.SetActive(true);
    }*/

    private void with_ammo_text(GameObject textNoAmmo)
    {
        textNoAmmo.SetActive(false);
    }

    public static void check_low_ammo(Gun currentGun, GameObject textLowAmmo)
    {
        if (currentGun.getGun.gunName != "Weapon" && currentGun.getGun.ammunition <= currentGun.getGun.maximumAmmo / 3 && currentGun.getGun.ammunition != 0)
            textLowAmmo.SetActive(true);
        else
            textLowAmmo.SetActive(false);
    }

    public static void check_no_ammo(Gun currentGun, GameObject textLowAmmo, GameObject textNoAmmo)
    {
        if (currentGun.getGun.ammunition == 0 && currentGun.getGun.gunName != "Weapon") {
            textLowAmmo.SetActive(false);
            textNoAmmo.SetActive(true);
            //out_ammo_text(textLowAmmo, textNoAmmo);
        }
        else
            textNoAmmo.SetActive(false);
    }
}
