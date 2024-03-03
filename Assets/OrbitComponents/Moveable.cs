using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float speed = 50f;

    [SerializeField] GravitySource orbitSource;

    Vector3 direction = Vector3.forward;

    public void SetCourse(Vector3 target)
    {
        direction = (target - transform.position).normalized;
        transform.forward = direction;
        orbitSource = null;
    }

    public void SetOrbit(GravitySource source)
    {
        orbitSource = source;
    }

    // Update is called once per frame
    void Update()
    {
        if (orbitSource != null)
        {
            Vector3 delta = (transform.position - orbitSource.transform.position);
            float theta = speed / (2 * 3.1415f * delta.magnitude);

            var nd = Quaternion.AngleAxis(theta, Vector3.up) * delta;

            transform.position = transform.position - delta + nd;

        }
        else
        {
            transform.position += Time.deltaTime * direction * speed;
        }
    }

    //what kind of movement mechanics do we want?
    //path given cur pos/vel, planet grav loc, target pos
}
