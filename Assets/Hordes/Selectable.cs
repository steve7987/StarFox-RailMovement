using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public abstract string GetText();

    public abstract void TakeDamage(float amount);

    public virtual void SmartAction(Vector3 target)
    {

    }

    //get team enum??
}
