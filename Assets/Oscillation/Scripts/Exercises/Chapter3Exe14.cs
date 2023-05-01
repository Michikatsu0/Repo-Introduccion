using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe14 : MonoBehaviour
{
    Rigidbody rb;
    public float inclineAngle;
    public float frictionCoefficient;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate the gravitational force on the box
        Vector3 gravityForce = Physics.gravity * rb.mass;

        // Calculate the normal force on the box
        float normalForce = rb.mass * Mathf.Cos(inclineAngle * Mathf.Deg2Rad);

        // Calculate the friction force on the box
        Vector3 frictionForce = -rb.velocity.normalized * frictionCoefficient * normalForce;

        // Calculate the total force on the box
        Vector3 totalForce = gravityForce + frictionForce;

        // Apply the total force to the box
        rb.AddForce(totalForce, ForceMode.Force);
    }
}
