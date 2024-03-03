using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubShipController : MonoBehaviour
{
    Damageable currentTarget;

    DockingBay owner;

    public void SetInitialTarget(Damageable target, DockingBay owner)
    {
        Debug.Assert(target != null);
        currentTarget = target;

        this.owner = owner;


    }

    private void Update()
    {
        if (currentTarget != null && currentTarget.IsAlive())
        {
            if ((currentTarget.transform.position - transform.position).magnitude < 25)
            {
                GetComponent<WeaponsSystem>().OpenFire(currentTarget);
            }
            else
            {
                GetComponent<Moveable>().SetCourse(currentTarget.transform.position);
            }
   
        }
        else
        {
            if ((owner.transform.position - transform.position).magnitude < 5)
            {
                owner.ReturnFighter(this);
            }
            else
            {
                GetComponent<Moveable>().SetCourse(owner.transform.position);
            }
        }

        //move towards target

        //if in range, begin firing

        //if target dead, return to ship, or later, look for new target?
    }

}
