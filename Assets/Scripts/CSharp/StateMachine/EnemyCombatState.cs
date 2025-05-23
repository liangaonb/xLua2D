using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : BaseState
{
    public override void EnterState(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.isInCombat = true;
        Debug.Log($"{currentEnemy.name} enter CombatState");
    }

    public override void LogicUpdate()
    {
        if (!currentEnemy.checkCondition.CheckTarget())
        {
            currentEnemy.ChangeState(EnemyStates.Move);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void ExitState()
    {
        currentEnemy.isInCombat = false;
        Debug.Log($"{currentEnemy.name} exit CombatState");
    }
}
