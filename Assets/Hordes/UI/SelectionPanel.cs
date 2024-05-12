using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPanel : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;

    BuildingController currentTarget;

    public void SetTarget(BuildingController building)
    {
        currentTarget = building;
        text.text = building.data.buildingName;
    }

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
        if (currentTarget == null)
        {
            Debug.LogWarning("Trying to destroy null target");
            return;
        }

        currentTarget.DestroyBuilding();
        ClearTarget();

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
