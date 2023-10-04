using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sense : MonoBehaviour
{
    //Class that represents the sense of the turret

    public virtual bool isDetecting(Transform player){return false;}
}

