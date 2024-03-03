using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeter : MonoBehaviour
{
    [SerializeField] Transform farTargeter;
    [SerializeField] Transform playerShip;
    [SerializeField] Transform gameplayPlane;

    [SerializeField] float farTargeterDistance = 5f;

    [SerializeField] float targeterMoveSpeed = 5f;
    [SerializeField] float resetConst = 0.9f;

    // Update is called once per frame
    void Update()
    {
        //put targeter infront of ships current position

        //transform.position = playerShip.position + gameplayPlane.forward * 17f;



        
        //adjust this targeter based on input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        //clamp its position to the screen
        Camera main = Camera.main;
        Vector3 pos = main.WorldToViewportPoint(transform.position);
       
        pos += new Vector3(x, y, 0) * targeterMoveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);


        //now pull it slightly towards spot in front of the ship
        Vector3 baseTargeterPosition = main.WorldToViewportPoint(playerShip.position + gameplayPlane.forward * 17f);
        
        //maybe only do this if no input??
        float t = Mathf.Pow(resetConst, Time.deltaTime);
        pos.x = t * pos.x + (1 - t) * baseTargeterPosition.x;
        pos.y = t * pos.y + (1 - t) * baseTargeterPosition.y;
        
        

        transform.position = main.ViewportToWorldPoint(pos);
        
        


        //calc position of far targeter

        farTargeter.position = (transform.position - playerShip.position).normalized * farTargeterDistance + transform.position;
    }
}
