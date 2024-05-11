using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighterCube : MonoBehaviour
{
    [SerializeField] GameObject red;
    [SerializeField] GameObject green;
    [SerializeField] GameObject cyan;

    public void SetDisplayType(HighlightTypes highlight)
    {
        red.SetActive(false);
        green.SetActive(false);
        cyan.SetActive(false);
        switch (highlight)
        {
            case HighlightTypes.off:
                break;
            case HighlightTypes.buildable:
                green.SetActive(true);
                break;
            case HighlightTypes.unbuildable:
                red.SetActive(true);
                break;
            case HighlightTypes.damper:
                cyan.SetActive(true);
                break;

        }
    }
}
