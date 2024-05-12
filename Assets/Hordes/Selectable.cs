using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public abstract string GetText();

    public abstract void TakeDamage(float amount);

    //get team enum??
}
