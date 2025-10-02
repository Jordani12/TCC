using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewEnemyData", menuName ="Enemy/Enemy Data")]
public class SOEnemy : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth;
    public float movementSpeed;
    public int maxAmmo;
    public int damage;
    public float attackRange;
    public float attackRate;

    [Header("Type")]
    public bool isItGun;
    public bool isBat;

    [Header("Rewards")]
    public int scoreValue;
}
