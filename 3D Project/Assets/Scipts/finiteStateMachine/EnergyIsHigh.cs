using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/EnergyIsHigh")]
public class EnergyIsHigh : Condition
{
    [SerializeField] private bool hasEnergy;

    public override bool Test(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().energy >=90 )
        {
            return hasEnergy;
        }
        else
        {
            return !hasEnergy;
        }
    }
}
