using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITargeter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text scannerText;
    [SerializeField] GameObject targeterEffect;
    [SerializeField] float radius = 12.5f;
    

    Scanable currentTarget;  //what's displayed on the text panel

    PlayerAction currentAction = PlayerAction.None;

    Material material;

    private void Awake()
    {
        material = targeterEffect.GetComponent<MeshRenderer>().material;
    }

    public void SetTargeterPosition(Vector3 position)
    {
        transform.position = position;

        switch (currentAction)
        {
            case PlayerAction.None:
            case PlayerAction.Scan:
                SetCurrentTarget(GetClosestComponent<Scanable>());
                break;
            case PlayerAction.Fire:
                var d = GetClosestComponent<Damageable>();
                if (d != null)
                {
                    SetCurrentTarget(d.GetComponent<Scanable>());
                }
                else
                {
                    SetCurrentTarget(null);
                }
                break;
            case PlayerAction.Heading:
                //maybe update current target if its orbital?
                var g = GetClosestComponent<GravitySource>();
                if (g != null)
                {
                    SetCurrentTarget(g.GetComponent<Scanable>());
                }
                break;
            default:
                break;
        }

    }

    void SetCurrentTarget(Scanable nextTarget)
    {
        currentTarget = nextTarget;
    }

    private void Update()
    {
        //Debug.Log(currentTarget);
        if (currentTarget != null)
        {
            //put an outline on the target?
            scannerText.text = "Current Target:\n" + currentTarget.GetScanKnowledge();
        }
    }

    public void SetTargeterType(PlayerAction action)  //maybe also size?
    {
        //exiting
        switch (currentAction)
        {
            case PlayerAction.None:
                Cursor.visible = false;
                break;
            case PlayerAction.Scan:
                targeterEffect.SetActive(false);
                break;
            case PlayerAction.Fire:
                targeterEffect.SetActive(false);
                break;
            case PlayerAction.Heading:
                targeterEffect.SetActive(false);
                break;
            case PlayerAction.LaunchShips:
                targeterEffect.SetActive(false);
                break;
            default:
                break;
        }

        //entering
        switch (action)
        {
            case PlayerAction.None:
                Cursor.visible = true;
                break;
            case PlayerAction.Scan:
                targeterEffect.SetActive(true);
                material.color = Color.grey;
                break;
            case PlayerAction.Fire:
                targeterEffect.SetActive(true);
                material.color = Color.red;
                break;
            case PlayerAction.Heading:
                targeterEffect.SetActive(true);
                material.color = Color.blue;
                break;
            case PlayerAction.LaunchShips:
                targeterEffect.SetActive(true);
                material.color = Color.cyan;
                break;
        }

        currentAction = action;
    }

    /// <summary>
    /// Does an overlap sphere to find all components in the radius.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="radius"></param>
    /// <returns>The clostest component, or null</returns>
    public T GetClosestComponent<T>()
    {
        Vector3 target = transform.position;
        var hits = Physics.OverlapSphere(target, radius);

        T ret = default(T);
        float bestD = Mathf.Infinity;

        foreach (var c in hits)
        {
            var s = c.GetComponent<T>();
            if (s != null)
            {
                float d = (target - c.transform.position).sqrMagnitude;
                if (d < bestD)
                {
                    bestD = d;
                    ret = s;
                }
            }
        }

        return ret;
    }
}
