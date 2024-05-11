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

    //methods

    public Vector3 GetSpriteScale()
    {
        var lx = 135f * buildingSize.x / buildingImage.texture.width;
        var ly = lx;

        return new Vector3(lx, ly, 1);
    }
}
