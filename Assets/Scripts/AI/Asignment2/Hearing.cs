using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    private Rigidbody playerRigidBody;
    private TurretController turretController;
    private float hearingRange = 15f;
    private void FindTank()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        playerRigidBody = player.GetComponent<Rigidbody>();
        turretController= GetComponent<TurretController>();
    }

    private void Update()
    {
        if(playerTransform == null) { FindTank(); }

        if(Vector2.Distance(transform.position, playerTransform.position) < hearingRange)
        {
            turretController.Detecting();
        }
    }
}
