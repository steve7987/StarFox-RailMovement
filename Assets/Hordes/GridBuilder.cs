using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] BuildingController buildingPrefab;

    GridManager gridManager;
    
    

    private void Awake()
    {
        gridManager = new GridManager(100, 100, this);
    }


    public void CreateBuilding(BuildingData data, Vector3 worldPos, bool useResources)
    {
        Vector3 pos = GridDrawer.WorldToGridPosition(worldPos);
     
        if (!gridManager.CanAddBuilding(pos, data))
        {
            Debug.LogWarning("Can't build here");
            return;
        }
        if (useResources && (!ResourceManager.instance.HasResource(ConsumableResource.Ore, data.oreCost)
         || !ResourceManager.instance.HasResource(ConsumableResource.Rare, data.rareCost)
         || !ResourceManager.instance.HasResource(FlowResource.Worker, -data.workerSupply)
         || !ResourceManager.instance.HasResource(FlowResource.Power, -data.powerSupply)))
        {
            Debug.LogWarning("Not enough resources");
            return;
        }
        
        var build = Instantiate(buildingPrefab, pos, Quaternion.identity);
        build.Setup(data, !useResources);

        //adjust consumable resources
        if (useResources)
        {
            ResourceManager.instance.AddResource(ConsumableResource.Ore, -data.oreCost);
            ResourceManager.instance.AddResource(ConsumableResource.Rare, -data.rareCost);
        }

        gridManager.AddBuilding(pos, build);
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
