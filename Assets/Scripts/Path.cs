using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    /*i want to have:
    
    */

    [SerializeField] private Color debugColor = Color.red;
    public float radius = 2.0f;
    public Vector3[] pathPoints;

    public int Length{
        get{
            return pathPoints.Length;
        }
    } 

    public Vector3 GetPoint(int index){
        return pathPoints[index % Length];        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pathPoints.Length ; i++)
        {
            if (i<pathPoints.Length-1){

                Debug.DrawLine(pathPoints[i], pathPoints[i + 1], debugColor);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
