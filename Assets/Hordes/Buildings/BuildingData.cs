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
    public Vector2Int buildingSize = new Vector2Int(2, 2);

    //cost
    [Header("Costs")]
    public float buildTime = 15f;

    //produces

    //upkeep


    //methods

    public Vector3 GetSpriteScale()
    {
        var lx = 135f * buildingSize.x / buildingImage.texture.width;
        var ly = lx;

        return new Vector3(lx, ly, 1);
    }
}
