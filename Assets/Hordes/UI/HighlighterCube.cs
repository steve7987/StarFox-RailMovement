using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighterCube : MonoBehaviour
{
    [SerializeField] GameObject red;
    [SerializeField] GameObject green;

    public void SetBuildable(bool canBuild)
    {
        red.SetActive(!canBuild);
        green.SetActive(canBuild);
    }
}
