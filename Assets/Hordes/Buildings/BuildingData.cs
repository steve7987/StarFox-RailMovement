using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Building", menuName = "Buildings/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    [Header("Cosmetic")]
    public string buildingName;
    public Sprite buildingImage;
    public float colliderHeight = 1f;
    public float healthBarWidth = 160f;
    public Vector2 healthBarOffset = new Vector2(0, 2);
    public Vector2Int buildingSize = new Vector2Int(2, 2);
    public bool dragBuild = false;

    //some area display info
    //e.g. damper field, or ore mine resources?

    [Header("Damper")]
    public bool showDamperField = false;
    public float damperRange = 0;

    //cost
    [Header("Costs")]
    public float buildTime = 15f;
    public float oreCost = 5f;
    public float rareCost = 0f;

    //produces
    [Header("Production")]
    public float oreGen = 0;
    public float rareGen = 0;

    //upkeep
    [Header("Supply")]
    public float workerSupply = 0;
    public float powerSupply = -1;

    [Header("Training")]
    public UnitData trainableUnit; 

    [Header("Combat")]
    public float maxHitPoints = 50;
    public float attackRange = 0f;
    public float attackDamage = 6f;
    public float attackSpeed = 0.7f;


    //methods

    public Vector3 GetSpriteScale()
    {
        //why is this 135f??? 
        var lx = 135f * buildingSize.x / buildingImage.texture.width;
        var ly = lx;

        return new Vector3(lx, ly, 1);
    }
}
