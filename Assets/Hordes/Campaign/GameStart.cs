using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] BuildingData buildingData;

    // Start is called before the first frame update
    void Start()
    {
        GridManager.instance.LoadGame(buildingData);
    }
}
