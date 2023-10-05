using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private Transform playerTransform;
    [SerializeField] private Transform turret;
    private float currRotSpeed = 1f;
    private float timer = 0f;
    private float shootRate = 5f;
    private bool sensed;

    private void FindComponent()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) { FindComponent(); }

        timer += Time.deltaTime;
        if (sensed)
        {
            Vector3 destinationPos = playerTransform.position;

            Quaternion turretRotation = Quaternion.LookRotation(destinationPos - turret.position);
            turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * currRotSpeed);
        }

        if(timer>1f)
        {
            sensed = false;
        }

    }

    public void Detecting()
    {
        sensed = true;
        timer = 0f;
    }
}
