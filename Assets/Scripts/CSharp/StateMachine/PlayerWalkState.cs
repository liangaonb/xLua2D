using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : BaseState<PlayerController>
{
    public override void EnterState(PlayerController player)
    {
        currentCharacter = player;
        player.moveSpeed = player.walkSpeed;
    }

    public override void LogicUpdate()
    {
        if (GameStateManager.Instance.currentGameState == GameState.Playing)
        {
            currentCharacter.ChangeState(PlayerStates.Run);
        }
    }

    public override void PhysicsUpdate()
    {
        // 低速向前移动
        currentCharacter.AutoMove();
    }

    public override void ExitState()
    {
        //Debug.Log("Exit Walk State");
    }
}