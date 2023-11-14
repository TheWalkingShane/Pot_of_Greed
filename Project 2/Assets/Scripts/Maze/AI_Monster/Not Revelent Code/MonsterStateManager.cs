using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStateManager : MonoBehaviour
{
    private MonsterBaseState currentState;
    public MonsterRoamState RoamState = new MonsterRoamState();
    public MonsterToPlayerState ToPlayerState = new MonsterToPlayerState();

    // Start is called before the first frame update
    void Start()
    {
        // starting state for the state machine
        currentState = RoamState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(MonsterBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
