using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : MonoBehaviour
{

    private TurretController turretController;

    // Start is called before the first frame update
    void Start()
    {
        turretController = GetComponent<TurretController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        BadSmell otherSmell = other.GetComponent<BadSmell>();
        if (otherSmell != null)
        {
            turretController.Detecting();
        }
    }
}
