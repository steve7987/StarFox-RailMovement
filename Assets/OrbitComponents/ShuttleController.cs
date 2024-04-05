using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleController : MonoBehaviour
{
    Landable currentTarget;

    DockingBay owner;

    enum ShuttleState
    {
        Departing,
        AtTarget,
        Returning,
    }

    ShuttleState currentState;

    public void SetInitialTarget(Landable target, DockingBay owner)
    {
        Debug.Assert(target != null);
        currentTarget = target;

        this.owner = owner;

        currentState = ShuttleState.Departing;

    }

    private void Update()
    {
        if (currentTarget == null)
        {
            currentState = ShuttleState.Returning;
        }
        
        if (currentState == ShuttleState.Departing)
        {
            if ((currentTarget.transform.position - transform.position).magnitude < 5)
            {
                currentState = ShuttleState.AtTarget;
            }
            else
            {
                GetComponent<Moveable>().SetCourse(currentTarget.transform.position);
            }
        }
        else if (currentState == ShuttleState.AtTarget)
        {
            //delay for some amount of time?
            currentState = ShuttleState.Returning;
        }
        else if (currentState == ShuttleState.Returning)
        {
            if ((owner.transform.position - transform.position).magnitude < 5)
            {
                owner.ReturnShuttle(this);
            }
            else
            {
                GetComponent<Moveable>().SetCourse(owner.transform.position);
            }
        }
    }
}
