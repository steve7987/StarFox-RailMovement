using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : Selectable
{
    public const float resourceGenerationInterval = 7f;

    [SerializeField] Color constructionColor;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Attacks")]
    [SerializeField] WeaponTargeter weaponTargeter;
    //[SerializeField] SphereCollider rangeCollider;

    [Header("Health Bar")]
    [SerializeField] RectTransform canvas;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider progressBar;  //work on progress bar display rules, and in combo with health bar

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

        //setup canvas and health bar and progress bar
        canvas.localPosition = new Vector3(data.healthBarOffset.x, data.healthBarOffset.y, 0);
        var rt = healthBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(data.healthBarWidth, rt.sizeDelta.y);
        rt = progressBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(data.healthBarWidth * 0.9f, rt.sizeDelta.y);

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
            weaponTargeter.SetSize(data.attackRange);
        }
        else
        {
            Destroy(weaponTargeter.gameObject);
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
            healthBar.value = currentHitPoints / data.maxHitPoints;
            progressBar.value = timeElapsed / data.buildTime;
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

        //hide health bar, will re-enable later if building takes damage? what about progress bar
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
        if (data.attackRange > 0)
        {
            StartCoroutine(WeaponFireCoroutine());
        }
    }

    IEnumerator WeaponFireCoroutine()
    {
        while (true)
        {
            if (weaponTargeter.target != null)
            {
                weaponTargeter.target.TakeDamage(data.attackDamage);
                yield return new WaitForSeconds(data.attackSpeed);
            }
            else
            {
                yield return null;
            }
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

    public override void TakeDamage(float amount)
    {
        currentHitPoints -= amount;
        canvas.gameObject.SetActive(true);
        healthBar.value = currentHitPoints / data.maxHitPoints;
        if (currentHitPoints <= 0)
        {
            DestroyBuilding();
        }
    }

    //also make a refund building as an ability?
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

    public override int GetNumActions()
    {
        return data.trainableUnit == null ? 0 : 1;
    }

    int unitsRemaining = 0;

    public override void Action1()
    {
        //train unit
        if (data.trainableUnit != null)
        {
            unitsRemaining += 1;
            if (unitsRemaining == 1)
            {
                StartCoroutine(TrainUnitCoroutine());
            }
            
        }
    }

    IEnumerator TrainUnitCoroutine()
    {
        canvas.gameObject.SetActive(true);
        while (unitsRemaining > 0)
        {
            float elapsed = 0;
            while (elapsed < data.trainableUnit.trainTime)
            {
                elapsed += Time.deltaTime;
                progressBar.value = elapsed / data.trainableUnit.trainTime;
                yield return null;
            }
            Debug.Log("Create " + data.trainableUnit.unitName);
            Instantiate(data.trainableUnit.prefab, transform.position, Quaternion.identity);
            progressBar.value = 0f;
            unitsRemaining -= 1;
        }
        if (healthBar.value >= 0.99f)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public override void Action2()
    {
        //refund building? research tech?
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

    public override string GetText()
    {
        return data.buildingName;
    }
}
