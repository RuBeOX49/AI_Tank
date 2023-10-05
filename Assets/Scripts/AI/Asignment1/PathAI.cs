using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAI : MonoBehaviour
{
    public float radius = 2.0f;
    public Vector3[] pathPoint;
    public Color _color = Color.red;
    public float Length
    {
        get { return pathPoint.Length; }
    }

    public Vector3 GetPoint(int index){
        return pathPoint[index];        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pathPoint.Length ; i++)
        {
            if (i<pathPoint.Length-1){

                Debug.DrawLine(pathPoint[i], pathPoint[i + 1], _color);
            }
        }
    }
}
