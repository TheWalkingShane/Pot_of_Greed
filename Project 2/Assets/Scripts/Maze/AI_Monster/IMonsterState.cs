using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void OnEnterState();
    void OnUpdateState();
    void OnExitState();
}
