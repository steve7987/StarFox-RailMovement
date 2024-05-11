using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCameraController : MonoBehaviour
{
    [SerializeField] float speed = 12f;

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }

        

        MoveCamera(direction);
    }

    void MoveCamera(Vector2 direction)
    {
        Vector3 dir = new Vector3(direction.x, 0, direction.y);

        dir = Quaternion.Euler(0, 45, 0) * dir;

        transform.position += Time.deltaTime * speed * dir;
    }

}
