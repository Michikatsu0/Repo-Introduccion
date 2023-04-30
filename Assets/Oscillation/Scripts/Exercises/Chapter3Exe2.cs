using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe2 : MonoBehaviour
{
    [SerializeField] private Transform shotPoint;
    [SerializeField] private int numberOfMovers = 10;
    [SerializeField] private Vector2 impulseForce;
    [SerializeField] private Vector3 angularForce;
    private List<Mover3_E2> movers = new List<Mover3_E2>();

    void Start()
    {
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            float randomMass = Random.Range(.4f, 1f);
            Mover3_E2 mover = new Mover3_E2(randomMass, impulseForce, shotPoint.position);
            movers.Add(mover);
        }
    }

    void FixedUpdate() 
    {
        foreach (var mover in movers)
        {
            mover.body.AddForce(Physics.gravity, ForceMode.Force);
            mover.ApplyRotation(angularForce);
            mover.CheckEdges();
        }
    }

    
}
public class Mover3_E2
{
    public Rigidbody body;
    public Transform transform;
    private GameObject gameObject;

    private Vector2 maximumPos;

    private Vector3 acceleration;

    public Mover3_E2(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        // Create a primitive cube for each mover and destroy the box collider to avoid collisions
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.Destroy(gameObject.GetComponent<BoxCollider>());
        transform = gameObject.transform;

        // Instantiate a rigidbody for the mover to accept forces
        gameObject.AddComponent<Rigidbody>();
        body = gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;

        // Instantiate a renderer and provide a shader
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        // Modify the scale of the object to give correlation between size and mass
        gameObject.transform.localScale = new Vector3(randomMass, randomMass, randomMass);
        body.mass = randomMass;

        body.position = initialPosition; // Random location passed into the constructor
        body.AddForce(initialVelocity, ForceMode.Impulse);

        // Instantiate the window limits
        FindWindowLimits();
    }
    // Pass in the applied force and calculate a rotation based on the application of force
    public void ApplyRotation(Vector3 angularForce)
    {
        acceleration.x = angularForce.x / 2f;
        acceleration.y = angularForce.y / 2f;
        acceleration.z = angularForce.z / 2f;

        transform.Rotate(acceleration);
    }

    //Checks to ensure the body stays within the boundaries
    public void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (body.position.x > maximumPos.x || body.position.x < -maximumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime;
        }
        if (body.position.y > maximumPos.y || body.position.y < -maximumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime;
        }
        body.velocity = velocity;
    }

    // Find the edges of the screen
    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}