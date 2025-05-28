using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : BaseState<Enemy>
{
    public override void EnterState(Enemy enemy)
    {
        currentCharacter = enemy;
        currentCharacter.isInCombat = true;
        //Debug.Log($"{currentCharacter.name} enter CombatState");
    }

    public override void LogicUpdate()
    {
        if (!currentCharacter.checkCondition.CheckTarget())
        {
            currentCharacter.ChangeState(EnemyStates.Move);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void ExitState()
    {
        currentCharacter.isInCombat = false;
        //Debug.Log($"{currentCharacter.name} exit CombatState");
    }
}
