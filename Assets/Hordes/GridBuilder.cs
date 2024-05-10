using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] BuildingController buildingPrefab;

    GridManager gridManager;

    private void Awake()
    {
        gridManager = new GridManager(100, 100);
    }


    public void CreateBuilding(BuildingData data, Vector3 worldPos)
    {
        Vector3 pos = GridDrawer.WorldToGridPosition(worldPos);
     
        if (!gridManager.CanAddBuilding(pos, data))
        {
            Debug.LogWarning("Can't build here");
            return;
        }
        if (!ResourceManager.instance.HasResource(ConsumableResource.Ore, data.oreCost)
         || !ResourceManager.instance.HasResource(ConsumableResource.Rare, data.rareCost)
         || !ResourceManager.instance.HasResource(FlowResource.People, -data.workerSupply)
         || !ResourceManager.instance.HasResource(FlowResource.Power, -data.powerSupply))
        {
            Debug.LogWarning("Not enough resources");
            return;
        }
        
        var build = Instantiate(buildingPrefab, pos, Quaternion.identity);
        build.Setup(data);

        //adjust resources
        ResourceManager.instance.AddResource(ConsumableResource.Ore, -data.oreCost);
        ResourceManager.instance.AddResource(ConsumableResource.Rare, -data.rareCost);

        ResourceManager.instance.AddResource(FlowResource.Power, data.powerSupply);
        ResourceManager.instance.AddResource(FlowResource.People, data.workerSupply);

        gridManager.AddBuilding(pos, data);
    }

    private void OnDrawGizmos()
    {
        if (gridManager != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (!gridManager.CanBuildSquare(i, j))
                    {
                        Gizmos.DrawCube(new Vector3(i + 0.5f, 0, j + 0.5f), new Vector3(0.9f, 0.05f, 0.9f));
                    }
                }
            }
        }
        
    }
}
