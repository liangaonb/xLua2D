using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : BaseState
{
    public override void EnterState(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.isInCombat = false;
        Debug.Log($"{currentEnemy.name} enter MoveState");
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.checkCondition.CheckTarget())
        {
            currentEnemy.ChangeState(EnemyStates.Combat);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isInCombat)
        {
            currentEnemy.Move();
        }
    }

    public override void ExitState()
    {
        Debug.Log($"{currentEnemy.name} exit MoveState");
    }
}
