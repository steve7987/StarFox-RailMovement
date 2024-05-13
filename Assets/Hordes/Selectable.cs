using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public abstract string GetText();

    public abstract void TakeDamage(float amount);

    /// <summary>
    /// Right click when this is selected
    /// </summary>
    /// <param name="target"></param>
    public virtual void SmartAction(Vector3 target)
    {

    }

    //what types of arguments show these actions have??
    public virtual void Action1()
    {

    }

    public virtual void Action2()
    {

    }

    public virtual int GetNumActions()
    {
        return 0;
    }

    //how do we do abilities, that is issuing actions to buildings/units
    //generic action1, action2 etc that we can override? or even show/not show UI for


    //get team enum??
}
