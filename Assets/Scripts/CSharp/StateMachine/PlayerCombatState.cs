using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : BaseState<PlayerController>
{
    public override void EnterState(PlayerController player)
    {
        currentCharacter = player;
        player.moveSpeed = 0;
        player.isInCombat = true;
    }

    public override void LogicUpdate()
    {
        // 检查周围是否还有敌人
        if (!currentCharacter.checkCondition.CheckTarget())
        {
            currentCharacter.ExitCombat();  // 如果没有敌人，退出战斗状态
        }
    }

    public override void PhysicsUpdate()
    {
        // 战斗状态下不移动
        currentCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public override void ExitState()
    {
        currentCharacter.isInCombat = false;
        Debug.Log("Exit Combat State");
    }
}
