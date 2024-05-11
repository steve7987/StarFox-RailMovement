using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableResource
{
    Ore,
    Rare,
}

public enum FlowResource
{
    Worker,
    Power,
}

public class ResourceManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text displayText;

    Dictionary<ConsumableResource, float> consumableResources;
    Dictionary<FlowResource, (float, float)> flowResources;

    public static ResourceManager instance;

    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;

        consumableResources = new Dictionary<ConsumableResource, float>();
        
        consumableResources.Add(ConsumableResource.Ore, 5000);
        consumableResources.Add(ConsumableResource.Rare, 1000);

        flowResources = new Dictionary<FlowResource, (float, float)>();

        flowResources.Add(FlowResource.Worker, (0, 1000));
        flowResources.Add(FlowResource.Power, (0, 1000));
    }

    private void Start()
    {
        UpdateResourceDisplay();
    }

    public bool HasResource(ConsumableResource r, float amount)
    {
        return consumableResources[r] >= amount;
    }

    public bool HasResource(FlowResource r, float amount)
    {
        if (amount <= 0)
        {
            return true;
        }

        return flowResources[r].Item1 + amount <= flowResources[r].Item2;
    }

    public void AddResource(ConsumableResource resource, float amount)
    {
        consumableResources[resource] += amount;
        Debug.Assert(consumableResources[resource] >= 0);

        UpdateResourceDisplay();
    }

    public void AddResource(FlowResource resource, float amount)
    {
        float nsup = flowResources[resource].Item2;
        float nused = flowResources[resource].Item1;
        if (amount > 0)
        {
            nsup += amount;
            
        }
        else
        {
            nused -= amount;
        }
        
        
        Debug.Assert(nused <= nsup);

        flowResources[resource] = (nused, nsup);

        UpdateResourceDisplay();

    }

    public void UpdateResourceDisplay()
    {
        //could make into an iteration?

        displayText.text = "Ore: " + consumableResources[ConsumableResource.Ore]
                       + ", REM: " + consumableResources[ConsumableResource.Rare]
                       + ", Workers: " + flowResources[FlowResource.Worker].Item1 + " / " + flowResources[FlowResource.Worker].Item2
                       + ", Power: " + flowResources[FlowResource.Power].Item1 + " / " + flowResources[FlowResource.Power].Item2;


    }
}
