using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAction
{
    None,
    Scan,
    Fire,
    Heading,
    LaunchShips,
}

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] GameObject playerShip;

    [Header("UI Hookups")]
    
    [SerializeField] UITargeter targeterUI;    

    PlayerAction currentActionType = PlayerAction.None;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeCurrentActionType(PlayerAction.Scan);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeCurrentActionType(PlayerAction.Fire);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeCurrentActionType(PlayerAction.Heading);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeCurrentActionType(PlayerAction.LaunchShips);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ChangeCurrentActionType(PlayerAction.None);
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch (currentActionType)
            {
                case PlayerAction.None:
                    var mt = GetPositionFromMouse();
                    targeterUI.SetTargeterPosition(mt);
                    break;
                case PlayerAction.Scan:
                    //want a delay here? how should that be handled?
                    //maybe we have a target that's displayed on the info panel
                    //and scanning just reveals more info about the target...
                    playerShip.GetComponent<ShipScanner>().PerformScan(targeterUI.GetClosestComponent<Scanable>());
                    ChangeCurrentActionType(PlayerAction.None);
                    break;
                case PlayerAction.Fire:
                    Damageable ct = targeterUI.GetClosestComponent<Damageable>();
                    if (ct != null)
                    {
                        playerShip.GetComponent<WeaponsSystem>().OpenFire(ct);
                    }
                    else
                    {
                        playerShip.GetComponent<WeaponsSystem>().CeaseFire();
                    }
                    ChangeCurrentActionType(PlayerAction.None);
                    break;
                case PlayerAction.Heading:
                    GravitySource gs = targeterUI.GetClosestComponent<GravitySource>();
                    if (gs != null)
                    {
                        playerShip.GetComponent<Moveable>().SetOrbit(gs);
                    }
                    else
                    {
                        playerShip.GetComponent<Moveable>().SetCourse(GetPositionFromMouse());
                    }
                    
                    ChangeCurrentActionType(PlayerAction.None);
                    break;
                case PlayerAction.LaunchShips:
                    //figure out what type to launch
                    Damageable d = targeterUI.GetClosestComponent<Damageable>();
                    if (d != null)
                    {
                        playerShip.GetComponent<DockingBay>().LaunchFighters(d, 3);
                    }

                    ChangeCurrentActionType(PlayerAction.None);
                    break;
                default:
                    break;
            }
        }

        //moves ui stuff
        switch (currentActionType)
        {
            case PlayerAction.Heading:
            case PlayerAction.LaunchShips:
            case PlayerAction.Fire:
            case PlayerAction.Scan:
                var mt = GetPositionFromMouse();
                targeterUI.SetTargeterPosition(mt);
                
                break;
            case PlayerAction.None:
            default:
                break;
        }

    }


    Vector3 GetPositionFromMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, Vector3.zero);

        float enter;
        if (plane.Raycast(ray, out enter))
        {
            return ray.GetPoint(enter);
        }
        else
        {
            Debug.LogWarning("No intersection");
            return Vector3.zero;
        }
    }

    void ChangeCurrentActionType(PlayerAction at)
    {
        //exiting
        switch (currentActionType)
        {
            case PlayerAction.None:

            case PlayerAction.Scan:

            case PlayerAction.Fire:

            default:
                break;
        }

        //entering
        switch (at)
        {
            case PlayerAction.None:

            case PlayerAction.Scan:

            case PlayerAction.Fire:

            default:
                break;
        }

        currentActionType = at;

        //needs to change UI
        targeterUI.SetTargeterType(currentActionType);
    }
}
