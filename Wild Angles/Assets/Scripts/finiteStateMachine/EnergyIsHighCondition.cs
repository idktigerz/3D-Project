using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/EnergyIsHigh")]
public class EnergyIsHighCondition : Condition
{
    [SerializeField] private bool hasEnergy;

    public override bool Test(FiniteStateMachine fsm)
    {
        if (!fsm.GetNavMeshAgent().owlSleeping)
        {
            return hasEnergy;
        }
        /* else if (fsm.GetNavMeshAgent().energy >= 90)
         {
             return hasEnergy;
         }
         else*/
        {
            return !hasEnergy;
        }
    }
}