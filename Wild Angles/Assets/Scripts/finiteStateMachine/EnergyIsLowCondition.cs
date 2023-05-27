using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/EnergyIsLow")]
public class EnergyIsLowCondition : Condition
{
    [SerializeField] private bool hasEnergy;

    public override bool Test(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().owlSleeping)
        {
            return !hasEnergy;
        }
        /*else if (fsm.GetNavMeshAgent().energy <= 30)
        {
            return !hasEnergy;
        }*/
        else return hasEnergy;
    }
}