using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    enum TurretStates {
        s_Searching,
        s_Idle
    }

    private Transform playerTransform;

    private Transform lastDetectedPosition;

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Sense sense;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float rotateSpeed = 20f;

    [SerializeField] private float shootAngle = 3f;

    [SerializeField] private float shootDelay = 1.5f;

    private float currentShootDelay = 0.0f;

    private TurretStates currentState = TurretStates.s_Idle;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TurretStates.s_Idle:
                break;
            case TurretStates.s_Searching:
                    Quaternion targetRotation = Quaternion.LookRotation(lastDetectedPosition.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

                    if (currentShootDelay <= 0){
                        float angle = Vector3.Angle(transform.forward, playerTransform.position - transform.position);
                        if(angle < shootAngle){
                            Shoot();
                        }
                    }
                
                break;
            default:
                break;
        }
        
        if(sense.isDetecting(playerTransform))
        {
            currentState = TurretStates.s_Searching;
            lastDetectedPosition = playerTransform;
        }
        else currentState = TurretStates.s_Idle;

        currentShootDelay -= Time.deltaTime;
        if(currentShootDelay < 0)
            currentShootDelay = 0;
    }

    private void Shoot()
    {
        //dispara
        GameObject shell = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        shell.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        currentShootDelay = 1.5f;
    }

}
