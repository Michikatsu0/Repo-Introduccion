using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe14 : MonoBehaviour
{
    Rigidbody rb;
    public float multiplier;
    public float inclineAngle;
    public float frictionCoefficient;
    private Vector3 totalForce;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate the gravitational force on the box
        // Calculate the normal force on the box
        Vector3 normalForce = Physics.gravity * rb.mass * Mathf.Cos(inclineAngle);

        // Calculate the friction force on the box
        Vector3 frictionForce = frictionCoefficient * normalForce;

        // Calculate the total force on the box
        Vector3 totalForce = multiplier * normalForce - frictionForce;

        // Apply the total force to the box
        rb.AddForce(totalForce, ForceMode.Force);
    }
}
