using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HearSense : Sense {

    [SerializeField] private TankShooting tShooting;
    [SerializeField] private float senseAngle = 90f;
    [SerializeField] private float senseRange = 20f;
    override public bool isDetecting(Transform player){
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        return angle < (senseAngle / 2)
         && Vector3.Distance(transform.position, player.position) < senseRange
         && tShooting.IsCharging;
         ;
    }
}