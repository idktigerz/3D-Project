using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/CanSleepOwl")]
public class CanSleepOwl : Condition
{
    [SerializeField] bool negation;

    public override bool Test(FiniteStateMachine fsm)
    {
        
        if (fsm.GetNavMeshAgent().timeController.currentTime.TimeOfDay > TimeSpan.FromHours(20) && fsm.GetNavMeshAgent().timeController.currentTime.TimeOfDay < TimeSpan.FromHours(6))
        {
            return negation;
        }
        {
            return !negation;
        }
    }
}