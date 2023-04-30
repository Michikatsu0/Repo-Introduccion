using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Chapter3Exe5 : MonoBehaviour
{
    Spacecraft spacecraft;
    public float rotationSpeed = 0.1f;
    void Start()
    {
        spacecraft = new Spacecraft(Vector3.zero);
    }
    private void Update()
    {
        spacecraft.SpaceCraftUpdate();
    }
    private void FixedUpdate()
    {
        spacecraft.UseForces();
    }
}
public class Spacecraft
{
    public Rigidbody body;
    public GameObject mover;
    public float thrustSpeed = 1f, turnDirection = 0;
    public bool thrusting;
    public Vector2 location, velocity, acceleration;

    private Vector2 maximumPos;

    public Vector3 rotationSpeed = new Vector3(0f, 0f, 0.1f);

    public Spacecraft(Vector3 position)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        body = mover.AddComponent<Rigidbody>();

        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        body.useGravity = false;

        FindWindowLimits();
    }

    public void UseForces()
    {
        if (thrusting)
        {
            body.AddForce(mover.transform.up * thrustSpeed);
            Debug.Log("Thrust");
        }
        if (turnDirection != 0f)
        {
            body.AddTorque(rotationSpeed * turnDirection);
        }
        location = mover.transform.position;

        CheckEdges();
    }
    public void SpaceCraftUpdate()
    {
        thrusting = Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1f;
            Debug.Log("Left Arrow");
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1f;
            Debug.Log("Right Arrow");
        }

        else
        {
            turnDirection = 0f;
        }
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
        }

        mover.transform.position = location;
    }

    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 8;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

}


