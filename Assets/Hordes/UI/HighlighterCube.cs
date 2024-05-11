using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighterCube : MonoBehaviour
{
    [SerializeField] Color buildableColor;
    [SerializeField] Color unbuildableColor;
    [SerializeField] Color damperColor;
    [SerializeField] Color buildingDamperColor;

    [SerializeField] GameObject cube;

    Material cm;

    private void Awake()
    {
        cm = cube.GetComponent<MeshRenderer>().material;
    }

    public void SetDisplayType(HighlightTypes highlight)
    {
        switch (highlight)
        {
            case HighlightTypes.off:
                cube.SetActive(false);
                break;
            case HighlightTypes.buildable:
                cube.SetActive(true);
                cm.color = buildableColor;
                break;
            case HighlightTypes.unbuildable:
                cube.SetActive(true);
                cm.color = unbuildableColor;
                break;
            case HighlightTypes.damper:
                cube.SetActive(true);
                cm.color = damperColor;
                break;
            case HighlightTypes.buildingDamper:
                cube.SetActive(true);
                cm.color = buildingDamperColor;
                break;

        }
    }
}
