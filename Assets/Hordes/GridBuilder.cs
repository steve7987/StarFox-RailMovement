using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] BuildingController buildingPrefab;


    public void CreateBuilding(BuildingData data, Vector3 worldPos)
    {
        Vector3 pos = GridDrawer.WorldToGridPosition(worldPos);
        pos.y = data.height;
        var build = Instantiate(buildingPrefab, pos, Quaternion.Euler(0, 45, 0));
        build.Setup(data);
    }
}
