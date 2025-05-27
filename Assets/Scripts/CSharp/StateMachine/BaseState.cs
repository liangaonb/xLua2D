using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> where T : MonoBehaviour
{
    protected T currentCharacter;
    public abstract void EnterState(T character);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void ExitState();
}
