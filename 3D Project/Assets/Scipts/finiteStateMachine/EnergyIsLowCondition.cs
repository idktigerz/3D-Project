using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/EnergyIsLow")]
public class EnergyIsLowCondition : Condition
{
    [SerializeField] private bool hasEnergy;

    public override bool Test(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().energy <= 49)
        {
            return !hasEnergy;
        }
        else
        {
            return hasEnergy;
        }
    }
}