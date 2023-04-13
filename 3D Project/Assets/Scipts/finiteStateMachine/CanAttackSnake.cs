using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/CanAttackSnake")]
public class CanAttackSnake : Condition
{
    [SerializeField] private bool negation;
    public override bool Test(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().snakeCanAttack)
        {
            return !negation;
        }
        return negation;
    }

}
