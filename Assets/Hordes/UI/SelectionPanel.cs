using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPanel : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;

    public void SetTarget(BuildingController building)
    {
        text.text = building.data.buildingName;
    }

    public void SetTarget(BuildingData data)
    {
        text.text = data.buildingName + "\nCosts: " + data.oreCost + " ore"; 
    }

    public void ClearTarget()
    {
        text.text = "";
    }
}
