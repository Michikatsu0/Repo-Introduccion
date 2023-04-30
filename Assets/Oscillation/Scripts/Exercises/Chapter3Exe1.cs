using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe1 : MonoBehaviour
{
    public GameObject baton;
    [SerializeField] float zAngleDegree = 60f;

    float zAngleRadian;
    Vector3 velocity;

    private void Start()
    {
        zAngleRadian = zAngleDegree * Mathf.Deg2Rad;
    }
    void Update()
    {
        velocity.z += zAngleRadian;
        baton.transform.Rotate(velocity, Space.World);    
    }
}