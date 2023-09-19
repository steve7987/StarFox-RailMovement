using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeter : MonoBehaviour
{
    [SerializeField] Transform farTargeter;
    [SerializeField] Transform playerShip;

    [SerializeField] float farTargeterDistance = 5f;

    [SerializeField] float targeterMoveSpeed = 5f;
    [SerializeField] float resetConst = 0.9f;

    // Update is called once per frame
    void Update()
    {


        //adjust this targeter based on input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //transform.localPosition += new Vector3(x, y, 0) * targeterMoveSpeed * Time.deltaTime;
        //Debug.Log(x + ", " + y);
            //clamp its position
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
       
        pos += new Vector3(x, y, 0) * targeterMoveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        //now pull it slightly towards 0.5f, 0.5f
        if (x == 0 && y == 0)
        {
            float t = Mathf.Pow(resetConst, Time.deltaTime);
            pos.x = t * pos.x + (1 - t) * 0.5f;
            pos.y = t * pos.y + (1 - t) * 0.5f;
        }
        

        transform.position = Camera.main.ViewportToWorldPoint(pos);

        


        //calc position of far targeter

        farTargeter.position = (transform.position - playerShip.position).normalized * farTargeterDistance + transform.position;
    }
}
