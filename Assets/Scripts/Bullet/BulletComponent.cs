using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player hit");
            Destroy(gameObject);
        }
    }
}
