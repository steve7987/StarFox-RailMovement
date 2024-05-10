using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text buttonText;

    public void Setup(int index, BuildingData data, MKInterpreter mKInterpreter)
    {
        GetComponent<Button>().onClick.AddListener(() => mKInterpreter.SetBuilding(index));

        buttonText.text = data.buildingName;
    }
}
