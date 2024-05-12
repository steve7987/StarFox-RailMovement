using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    public const float resourceGenerationInterval = 7f;

    [SerializeField] Color constructionColor;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Attacks")]
    [SerializeField] SphereCollider rangeCollider;

    [Header("Health Bar")]
    [SerializeField] RectTransform canvas;
    [SerializeField] Slider slider;

    public bool underConstruction { get; private set; }

    public BuildingData data { get; private set; }

    public float currentHitPoints;

    public void Setup(BuildingData data, bool skipConstruction)
    {
        this.data = data;

        //setup building sprite
        spriteRenderer.transform.localScale = data.GetSpriteScale();//new Vector3(lx, ly, 1);
        spriteRenderer.sprite = data.buildingImage;

        //also want to scale collider?
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.center = new Vector3(data.buildingSize.x / 2f, data.colliderHeight / 2f, data.buildingSize.y / 2f);
        
        bc.size = new Vector3(data.buildingSize.x, data.colliderHeight, data.buildingSize.y);

        //setup canvas and health bar
        canvas.localPosition = new Vector3(data.healthBarOffset.x, data.healthBarOffset.y, 0);
        var rt = slider.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(data.healthBarWidth, rt.sizeDelta.y);

        //start consuming resources
        if (data.powerSupply < 0)
        {
            ResourceManager.instance.AddResource(FlowResource.Power, data.powerSupply);
        }
        if (data.workerSupply < 0)
        {
            ResourceManager.instance.AddResource(FlowResource.Worker, data.workerSupply);
        }


        if (skipConstruction)
        {
            currentHitPoints = data.maxHitPoints;
            OnConstructionComplete();
        }
        else
        {
            //begin building itself
            StartCoroutine(ConstructionCorountine());
            currentHitPoints = 0;
        }

        if (data.attackRange > 0)
        {
            rangeCollider.radius = data.attackRange;
        }
        else
        {
            Destroy(rangeCollider.gameObject);
        }
    }

    IEnumerator ConstructionCorountine()
    {
        underConstruction = true;
        spriteRenderer.color = constructionColor;
        //yield return new WaitForSeconds(data.buildTime);
        if (data.damperRange > 0)
        {
            GridManager.instance.AddDampers(transform.position, data, true);
            GridDrawer.instance.RefreshDamperField();
        }
        

        float timeElapsed = 0;

        while (timeElapsed < data.buildTime)
        {
            currentHitPoints += Time.deltaTime * data.maxHitPoints / data.buildTime;
            timeElapsed += Time.deltaTime;
            slider.value = timeElapsed / data.buildTime;
            yield return null;
        }

        spriteRenderer.color = Color.white;
        OnConstructionComplete();
    }

    public void OnConstructionComplete()
    {
        underConstruction = false;
        if (data.damperRange > 0)
        {
            GridManager.instance.AddDampers(transform.position, data, false);
            GridDrawer.instance.RefreshDamperField();
        }

        //hide health bar, will re-enable later if building takes damage?
        canvas.gameObject.SetActive(false);

        //add in resources, start resource gen corountine if needed
        if (data.powerSupply > 0)
        {
            ResourceManager.instance.AddResource(FlowResource.Power, data.powerSupply);
        }
        if (data.workerSupply > 0)
        {
            ResourceManager.instance.AddResource(FlowResource.Worker, data.workerSupply);
        }

        if (data.oreGen != 0 || data.rareGen != 0)
        {
            StartCoroutine(ResourceGenCoroutine());
        }
    }

    IEnumerator ResourceGenCoroutine()
    {
        while (true)  //not destroyed?
        {
            yield return new WaitForSeconds(resourceGenerationInterval);
            ResourceManager.instance.AddResource(ConsumableResource.Ore, data.oreGen);
            ResourceManager.instance.AddResource(ConsumableResource.Rare, data.rareGen);
        }
    }

    //make this abstract?
    //or use separate damage component?
    public void TakeDamage(float amount)
    {
        currentHitPoints -= amount;
        canvas.gameObject.SetActive(true);
        slider.value = currentHitPoints / data.maxHitPoints;
        //Debug.Log(currentHitPoints);
        if (currentHitPoints <= 0)
        {
            DestroyBuilding();
        }
    }

    public void DestroyBuilding()
    {
        if (!underConstruction)
        {

            //need to update resources
            ResourceManager.instance.RemoveFlowResource(FlowResource.Power, data.powerSupply);
            ResourceManager.instance.RemoveFlowResource(FlowResource.Worker, data.workerSupply);
        }
        else
        {
            //since we are under construction, only remove drains
            //start consuming resources
            if (data.powerSupply < 0)
            {
                ResourceManager.instance.RemoveFlowResource(FlowResource.Power, data.powerSupply);
            }
            if (data.workerSupply < 0)
            {
                ResourceManager.instance.RemoveFlowResource(FlowResource.Worker, data.workerSupply);
            }
        }
        

        //alert grid manager to update 
        GridManager.instance.DestroyBuilding(this);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        
        Vector3 size = data == null ? new Vector3(2, 0.05f, 2) : new Vector3(data.buildingSize.x, 0f, data.buildingSize.y);

        Gizmos.color = Color.blue;

        Vector3 pos = transform.position;
        pos.y = 0;



        pos += size / 2f;

        Gizmos.DrawCube(pos, size);
        
    }
}
