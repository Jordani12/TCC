using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string objectName;
    public string objectDescription;
    public Enums.ItemType type;
    public bool AlreadyCollectThisItem;

    [Tooltip("Preencher apenas se [type] for [Ammo]")]
    public int value;

    [Tooltip("Preencher apenas se [type] for [Gun]")]
    public string weaponName;

}
