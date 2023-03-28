using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step2_2 : MonoBehaviour
{
    // Create a list to store multiple Movers
    List<Mover2_Step2_2> movers = new List<Mover2_Step2_2>();
    Attractor2_Step2_2 attractor;
    // Start is called before the first frame update
    void Start()
    {
        int numberOfMovers = 13;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            Mover2_Step2_2 m = new Mover2_Step2_2(Random.Range(0.4f, 1.4f), randomVelocity, randomLocation); //Each Mover is initialized randomly.
            movers.Add(m);
        }
        attractor = new Attractor2_Step2_2();
    }

    void FixedUpdate()
    {
        foreach (Mover2_Step2_2 m in movers)
        {
            Rigidbody body = m.body;
            Vector2 attractForce = attractor.Attract(body); // Apply the attraction from the Attractor on each Mover object
            Vector2 repelForce = attractor.Repel(body);
            m.ApplyForce(attractForce * 10);
            m.ApplyForce(repelForce * 0.08f);
            m.CalculatePosition();
        }

        for (int i = 0; i < movers.Count; i++)
        {
            for (int j = 0; j < movers.Count; j++)
            {
                if (i != j)
                {
                    // Now that we are sure that our Mover will not attract itself, we need it to attract a different Mover
                    // We do that by directing a mover to use their Attract() method on another mover Rigidbodys
                    Vector2 attractedMover = movers[j].Attract(movers[i].body);

                    // We then apply that force the mover with the Rigidbody's Addforce() method
                    movers[i].body.AddForce(attractedMover, ForceMode.Impulse);
                }
            }
            // Now we check the boundaries of our scene to make sure the movers don't fly off
            // When we use gravity, the Movers will naturally fall out of the camera's view
            // This stops that.
            movers[i].CalculatePosition();
        }
    }
}


public class Attractor2_Step2_2
{
    private float radius;
    private float mass;
    private float G;

    private Vector2 location;

    private Rigidbody body;
    public GameObject attractor;

    public Attractor2_Step2_2()
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(attractor.GetComponent<SphereCollider>());
        Renderer renderer = attractor.GetComponent<Renderer>();

        body = attractor.AddComponent<Rigidbody>();

        body.position = Vector3.zero;

        // Generate a radius
        radius = 4;

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        attractor.transform.position = body.position;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        attractor.transform.localScale = radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
        body.useGravity = false;
        body.isKinematic = true;

        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;
        renderer.enabled = false;

        G = 9.8f;
    }

    public Vector2 Attract(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (distance * distance) / (G * body.mass * m.mass);
        force *= strength;
        return force;
    }

    public Vector2 Repel(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (G * body.mass * m.mass) / (distance * distance);
        force *= -strength;
        return force;
    }

}

public class Mover2_Step2_2
{
    // The basic properties of a mover class
    public Rigidbody body;
    private Transform transform;
    private GameObject mover;

    private Vector2 maximumPos;

    public Mover2_Step2_2(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(mover.GetComponent<SphereCollider>());
        transform = mover.transform;

        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = false;

        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = randomMass;
        body.position = initialPosition; // Default location
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit
        FindWindowLimits();
    }

    public void ApplyForce(Vector2 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    public void CalculatePosition()
    {
        CheckEdges();
    }

    private void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < -maximumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < -maximumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime;
        }
        body.velocity = velocity;
    }

    public Vector2 Attract(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (9.81f * body.mass * m.mass) / (distance * distance);
        force *= strength * 0.1f;
        return force;
    }

    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 20;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
