using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerDirection;
    private Transform playerTransform;
    private TurretController turretController;
    private float fieldOfView = 120f;
    private float viewDistance = 20f;



    private void FindTank()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        turretController = GetComponent<TurretController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) 
            FindTank();
        DetectAspect();
    }

    private void DetectAspect()
    {
        playerDirection = playerTransform.position - transform.position;

        //Check angle between the NPC's forward direction and the direction to the player
        if (Vector3.Angle(playerDirection, transform.forward) < fieldOfView * 0.5f)
        {
            if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, viewDistance))
            {
                TankMovementAI hitAspect = hit.collider.GetComponent<TankMovementAI>();
                if (hitAspect != null)
                {
                    turretController.Detecting();
                        Debug.Log("Enemy sighted");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        float halfFOV = fieldOfView * 0.5f;

        Quaternion forwardRayRotation = Quaternion.AngleAxis(0f, Vector3.up);
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 forwardRayDirection = forwardRayRotation * transform.forward;
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forwardRayDirection * viewDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, rightRayDirection * viewDistance);
        Gizmos.DrawRay(transform.position, leftRayDirection * viewDistance);
    }
}
