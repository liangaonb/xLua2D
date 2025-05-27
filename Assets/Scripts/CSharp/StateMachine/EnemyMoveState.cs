using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : BaseState<Enemy>
{
    public override void EnterState(Enemy enemy)
    {
        currentCharacter = enemy;
        currentCharacter.isInCombat = false;
        Debug.Log($"{currentCharacter.name} enter MoveState");
    }

    public override void LogicUpdate()
    {
        if (currentCharacter.checkCondition.CheckTarget())
        {
            currentCharacter.ChangeState(EnemyStates.Combat);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentCharacter.isInCombat)
        {
            currentCharacter.Move();
        }
    }

    public override void ExitState()
    {
        Debug.Log($"{currentCharacter.name} exit MoveState");
    }
}
