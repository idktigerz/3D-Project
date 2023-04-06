using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Alerting")]
public class AlertingAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        GameObject[] listOfAnimals;
        listOfAnimals = GameObject.FindGameObjectsWithTag("Animal");
        foreach (var ani in listOfAnimals)
        {
            //meter o cacn see condition mas com cada animal e o tiger
        }

    }
}
