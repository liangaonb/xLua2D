using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : BaseState<PlayerController>
{
    public override void EnterState(PlayerController player)
    {
        currentCharacter = player;
        player.moveSpeed = player.runSpeed;
    }

    public override void LogicUpdate()
    {
        // 在Run状态下检测敌人
        if (currentCharacter.checkCondition.CheckTarget())
        {
            currentCharacter.isInCombat = true;
            currentCharacter.ChangeState(PlayerStates.Combat);
        }
    }

    public override void PhysicsUpdate()
    {
        // 保持高速向前移动
        currentCharacter.AutoMove();
    }

    public override void ExitState()
    {
        //Debug.Log("Exit Run State");
    }
}
