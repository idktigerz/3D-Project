using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/CanFollowCondition")]
public class CanFollowCondition : Condition
{
    [SerializeField] private bool negation;
    public override bool Test(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().following || fsm.GetNavMeshAgent().homming) return !negation; else return negation;
    }
}