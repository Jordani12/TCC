using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICowboyState
{
    void EnterState(CowboyStateManager Enemy);
    void UpdateState(CowboyStateManager Enemy);
    void ExitState(CowboyStateManager Enemy);
}
