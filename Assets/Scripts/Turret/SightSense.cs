using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SightSense : Sense {
    [SerializeField] private float sightAngle = 90f;
    [SerializeField] private float sightRange = 20f;
    override public bool isDetecting(Transform player){
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        return angle < (sightAngle / 2) && Vector3.Distance(transform.position, player.position) < sightRange;
    }
}