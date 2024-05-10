using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MKInterpreter : MonoBehaviour
{
    enum MKState
    {
        NoAction,
        Build,

        //drag select
        //etc?
    }

    [SerializeField] GridBuilder gridBuilder;

    [SerializeField] BuildHighlighter buildHighlighter;

    [SerializeField] List<BuildingData> availableBuildings;

    [SerializeField] Transform buildingPanel;
    [SerializeField] BuildingButton buildingButton;

    MKState currentState;
    int currentBuildIndex;

    private void Awake()
    {
        currentState = MKState.NoAction;
        buildHighlighter.Activate();
    }

    private void Start()
    {
        for (int i = 0; i < availableBuildings.Count; i++)
        {
            var but = Instantiate(buildingButton, buildingPanel);
            but.Setup(i, availableBuildings[i], this);
            
        }
    }

    private void Update()
    {
        if (currentState == MKState.Build)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the mouse position
            Plane plane = new Plane(Vector3.up, Vector3.zero); // Define a plane using its normal and a point on it

            float distance;
            if (plane.Raycast(ray, out distance)) // Check if the ray intersects the plane
            {
                Vector3 hitPoint = ray.GetPoint(distance); // Get the intersection point
                //Debug.Log("Intersection point: " + hitPoint);
                buildHighlighter.SetPosition(hitPoint);

                if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    //currentState = MKState.NoAction;
                    //buildHighlighter.DeActivate();
                    gridBuilder.CreateBuilding(availableBuildings[currentBuildIndex], hitPoint);
                    buildHighlighter.CheckBuildable();
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                currentState = MKState.NoAction;
                buildHighlighter.DeActivate();
            }
        }
    }


    public void SetBuilding(int index)
    {
        Debug.Log(index);

        currentState = MKState.Build;
        currentBuildIndex = index;
        buildHighlighter.Activate();

        buildHighlighter.SetSize(availableBuildings[index].buildingSize);

    }

}
