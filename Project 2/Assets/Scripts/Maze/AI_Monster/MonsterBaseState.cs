using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState
{
    public abstract void EnterState(MonsterStateManager monster);
	public abstract void UpdateState(MonsterStateManager monster);
	public abstract void OnCollisionEnter(MonsterStateManager monster);
}
