using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Building", menuName = "Buildings/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    [Header("Cosmetic")]
    public string buildingName;
    public Sprite buildingImage;
    public float height = 1f;

    public Vector2Int buildingSize = new Vector2Int(2, 2);

    //cost

    //produces

    //upkeep
}
