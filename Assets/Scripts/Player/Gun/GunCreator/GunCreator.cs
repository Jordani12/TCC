using UnityEngine;
[CreateAssetMenu(fileName = "GunCreator", menuName = "New Gun")]
public class GunCreator : ScriptableObject
{
    [Header("Name")]
    public string gunName;
    [Header("Damage")]
    public int damage;
    [Header("Time")]
    [Tooltip("Preencher apenas se for arma de fogo")] public float rechargingTime;
    public float shootingTime;
    [Header("Bullets")]
    [Tooltip("Preencher apenas se for arma de fogo")] public int maximumAmmo;
    [Tooltip("Preencher apenas se for arma de fogo")] public int ammunition;
    [Tooltip("Preencher apenas se for arma de fogo")] public int ammunitionToReload;
    [Header("Recoil")]
    [Tooltip("Preencher apenas se for arma de fogo")] public int recoilGun;
    [Header("Distance")]
    public float distance;
    [Header("CheckTypeGun")]
    public bool isMelee;

    public string Reload;
    public string Idle;
    public string Shoot;
}
