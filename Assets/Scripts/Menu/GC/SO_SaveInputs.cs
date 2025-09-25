using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]

public class SO_SaveInputs : ScriptableObject
{
    public KeyCode forward_in;
    public KeyCode backward_in;
    public KeyCode left_in;
    public KeyCode right_in;
    public KeyCode dash_in;
    public KeyCode finalization_in;
    public KeyCode interact_in;
    public float fov;
    public float sensitivityX;
    public float sensitivityY;
    public float aim_sensitivityX;
    public float aim_sensitivityY;
}
