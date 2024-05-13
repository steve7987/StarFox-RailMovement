using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPanel : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;

    public Selectable currentTarget { get; private set; }

    /// <summary>
    /// Displays this particular selectable thing
    /// </summary>
    /// <param name="building"></param>
    public void SetTarget(Selectable selectable)
    {
        if (selectable == null)
        {
            ClearTarget();
            return;
        }
        currentTarget = selectable;
        text.text = selectable.GetText();  //instead: get description from DS
    }

    /// <summary>
    /// Displays the building costs for the building
    /// </summary>
    /// <param name="data"></param>
    public void SetTarget(BuildingData data)
    {
        text.text = data.buildingName + "\nCosts: " + data.oreCost + " ore";
        currentTarget = null;
    }

    public void ClearTarget()
    {
        currentTarget = null;
        text.text = "";
    }

    public void DestroyTarget()
    {
        //how should this become the refund building command, but we can't refund troops?
        //need options in selectable

        /*
        if (currentTarget == null)
        {
            Debug.LogWarning("Trying to destroy null target");
            return;
        }

        currentTarget.DestroyBuilding();
        ClearTarget();
        */
    }

    public void DamageTarget()
    {
        if (currentTarget == null)
        {
            Debug.LogWarning("Trying to destroy null target");
            return;
        }

        currentTarget.TakeDamage(20);
    }

}
