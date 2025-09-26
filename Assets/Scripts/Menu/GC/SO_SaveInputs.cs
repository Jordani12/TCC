using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]

public class SO_SaveInputs : ScriptableObject
{
    [Header("Move Inputs")]
    public KeyCode forward_in;
    public KeyCode backward_in;
    public KeyCode left_in;
    public KeyCode right_in;
    [Header("Especial Inputs")]
    public KeyCode dash_in;
    public KeyCode finalization_in;
    public KeyCode interact_in;
    [Header("Gun Inputs")]
    public KeyCode recharging;
    public MouseButton aim;
    public KeyCode change1;
    public KeyCode change2;
    public KeyCode change3;

    [Header("FOV")]
    public float fov;
    [Header("Sensivity")]
    public float sensitivityX;
    public float sensitivityY;
    public float aim_sensitivityX;
    public float aim_sensitivityY;
}
