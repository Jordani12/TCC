using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadingEnemyState : ICowboyState
{
    public void EnterState(CowboyStateManager Enemy)
    {
        MonoBehaviour.Destroy(Enemy.gameObject);
    }

    public void ExitState(CowboyStateManager Enemy)
    {

    }

    public void UpdateState(CowboyStateManager Enemy)
    {

    }
}
