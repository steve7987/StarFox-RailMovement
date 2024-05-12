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

    [SerializeField] SelectionPanel selectionPanel;

    MKState currentState;
    int currentBuildIndex;

    private void Awake()
    {
        currentState = MKState.NoAction;
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

                if ((Input.GetMouseButtonDown(0) || (availableBuildings[currentBuildIndex].dragBuild && Input.GetMouseButton(0))) 
                    && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    //instead should build drag build stuff differently?
                    gridBuilder.CreateBuilding(availableBuildings[currentBuildIndex], hitPoint, true);
                    buildHighlighter.Activate(availableBuildings[currentBuildIndex]);
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                currentState = MKState.NoAction;
                buildHighlighter.DeActivate();
                selectionPanel.ClearTarget();
            }
        }
        else if (currentState == MKState.NoAction)
        {
            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the mouse position
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Selectable")))
                {
                    //Debug.Log("Select: " + hit.collider.GetComponent<BuildingController>().data.buildingName);
                    selectionPanel.SetTarget(hit.collider.GetComponent<BuildingController>());
                    //
                }
                else
                {
                    selectionPanel.ClearTarget();
                }
            }
        }
    }


    public void SetBuilding(int index)
    {
        //Debug.Log(index);

        currentState = MKState.Build;
        currentBuildIndex = index;
        selectionPanel.SetTarget(availableBuildings[index]);

        buildHighlighter.Activate(availableBuildings[index]);

    }


    public void SaveGame()
    {
        GridManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        GridManager.instance.LoadGame(availableBuildings[0]);
    }

}
