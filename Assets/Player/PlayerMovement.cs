using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerModel;

    [Header("Settings")]
    public bool joystick = true;

    [Space]

    [Header("Parameters")]
    public float xySpeed = 18;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;
    public float xMax = 12f;
    public float yMax = 8f;
    public float lookahead = 3f;
    public float sightsDistance = 20f;
    public float weaponRange = 24f;
    public float laserDisplayTime = 0.1f;
    public float laserCooldownTime = 0.1f;

    [Space]

    [Header("Public References")]
    public Transform gameplayPlane;
    public CinemachineDollyCart dolly;
    public Transform cameraParent;
    [SerializeField] GameObject leftLaser;
    [SerializeField] GameObject rightLaser;
    [SerializeField] Transform farTargeter;


    Coroutine fireWeaponsCoroutine;

    void Start()
    {
        playerModel = transform.GetChild(0);
        SetSpeed(forwardSpeed);
        Cursor.visible = false;

        leftLaser.SetActive(false);
        rightLaser.SetActive(false);
    }

    void Update()
    {
        float h = joystick ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float v = joystick ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");

        LocalMove(h, v, xySpeed);
        RotationLook(h,v, lookSpeed);
        HorizontalLean(playerModel, h, 80, .1f);

        if (Input.anyKey)
        {
            FireWeapons();
        }
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 localPos = transform.localPosition;

        localPos.x = Mathf.Clamp(localPos.x, -xMax, xMax);
        localPos.y = Mathf.Clamp(localPos.y, -yMax, yMax);

        transform.localPosition = localPos;

        

        /*
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        */
    }

    void RotationLook(float h, float v, float speed)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, gameplayPlane.transform.rotation * Quaternion.LookRotation(new Vector3(h, v, lookahead)), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    void SetSpeed(float x)
    {
        dolly.m_Speed = x;
    }

    void FireWeapons()
    {
        if (fireWeaponsCoroutine == null)
        {
            fireWeaponsCoroutine = StartCoroutine(TriggerWeapons());
        }
    }

    IEnumerator TriggerWeapons()
    {
        leftLaser.SetActive(true);
        rightLaser.SetActive(true);

        //orient lasers
        Vector3 sightsTarget = (farTargeter.position - transform.position).normalized * sightsDistance + transform.position;
        leftLaser.transform.forward = sightsTarget - leftLaser.transform.position;
        rightLaser.transform.forward = sightsTarget - rightLaser.transform.position;

        //now raycast?
        RaycastHit hit;
        if (Physics.Raycast(leftLaser.transform.position, leftLaser.transform.forward, out hit, weaponRange, 1 << LayerMask.NameToLayer("Damagable")))
        {
            //Debug.Log("hit");
            hit.collider.GetComponent<DamagableObject>().GetHit();
        }
        if (Physics.Raycast(rightLaser.transform.position, rightLaser.transform.forward, out hit, weaponRange, 1 << LayerMask.NameToLayer("Damagable")))
        {
            //Debug.Log("hit");
            hit.collider.GetComponent<DamagableObject>().GetHit();
        }
        //Debug.DrawRay(leftLaser.transform.position, leftLaser.transform.forward * 24f, Color.magenta, 4f);


        yield return new WaitForSeconds(laserDisplayTime);


        leftLaser.SetActive(false);
        rightLaser.SetActive(false);

        yield return new WaitForSeconds(laserCooldownTime);
        fireWeaponsCoroutine = null;
    }

    /*
    void SetCameraZoom(float zoom, float duration)
    {
        //cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
    }

    void FieldOfView(float fov)
    {
        cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
    }

    
    void Boost(bool state)
    {

        if (state)
        {
            cameraParent.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
            trail.Play();
            circle.Play();
        }
        else
        {
            trail.Stop();
            circle.Stop();
        }
        trail.GetComponent<TrailRenderer>().emitting = state;

        float origFov = state ? 40 : 55;
        float endFov = state ? 55 : 40;
        float origChrom = state ? 0 : 1;
        float endChrom = state ? 1 : 0;
        float origDistortion = state ? 0 : -30;
        float endDistorton = state ? -30 : 0;
        float starsVel = state ? -20 : -1;
        float speed = state ? forwardSpeed * 2 : forwardSpeed;
        float zoom = state ? -7 : 0;

        DOVirtual.Float(origChrom, endChrom, .5f, Chromatic);
        DOVirtual.Float(origFov, endFov, .5f, FieldOfView);
        DOVirtual.Float(origDistortion, endDistorton, .5f, DistortionAmount);
        var pvel = stars.velocityOverLifetime;
        pvel.z = starsVel;

        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        SetCameraZoom(zoom, .4f);
    }

    void Break(bool state)
    {
        float speed = state ? forwardSpeed / 3 : forwardSpeed;
        float zoom = state ? 3 : 0;

        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        SetCameraZoom(zoom, .4f);
    }
    */
}
