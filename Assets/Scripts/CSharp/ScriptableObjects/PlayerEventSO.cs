using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerEventSO")]
public class PlayerEventSO : ScriptableObject
{
    public UnityAction<Player> OnEventRaised;

    public void RaiseEvent(Player player)
    {
        OnEventRaised?.Invoke(player);
    }
}
