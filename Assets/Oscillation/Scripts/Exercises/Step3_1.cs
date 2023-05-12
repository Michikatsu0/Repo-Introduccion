using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Step3_1 : MonoBehaviour
{
    // Create a list to store multiple Movers
    [SerializeField] float delay;
    List<Mover3_Step3_1> movers = new List<Mover3_Step3_1>();
    List<Attractor2_Step3_2> l_attractors = new List<Attractor2_Step3_2>();
    float time = 0;
    int attractorIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        int numberOfMovers = 40;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            Mover3_Step3_1 m = new Mover3_Step3_1(Random.Range(0.2f, 1f), randomVelocity, randomLocation); //Each Mover is initialized randomly.
            movers.Add(m);
        }

        int nomberOfAtractors = 1;
        for (int i = 0; i < nomberOfAtractors; i++)
        {
            Attractor2_Step3_2 a = new Attractor2_Step3_2();
            l_attractors.Add(a);
        }
        foreach (Attractor2_Step3_2 attractor in l_attractors)
        {
            attractor.attractor.SetActive(false);
        }
    }

    private void Update()
    {
        if (time >= delay)
        {
            attractorIndex++;
            time = 0;
        }
        time += Time.deltaTime;

        foreach (Attractor2_Step3_2 attractor in l_attractors)
        {
            attractor.attractor.SetActive(false);
        }

        if (attractorIndex == l_attractors.Count)
            attractorIndex = 0;

        l_attractors[attractorIndex].attractor.SetActive(true);

        float x =Mathf.Cos(l_attractors[attractorIndex].angle.x) * l_attractors[attractorIndex].amplitude.x;

        //Each oscillator object oscillating on the y-axis
        float y = Mathf.Cos(l_attractors[attractorIndex].angle.y) * l_attractors[attractorIndex].amplitude.y;

        l_attractors[attractorIndex].angle += l_attractors[attractorIndex].body.velocity;

        l_attractors[attractorIndex].attractor.transform.Translate(new Vector2(x, y) * Time.deltaTime);


        l_attractors[attractorIndex].CheckEdges();
    }

    void FixedUpdate()
    {
        foreach (Mover3_Step3_1 m in movers)
        {
            Rigidbody body = m.body;
            Vector2 force = l_attractors[attractorIndex].Attract(body); // Apply the attraction from the Attractor on each Mover object

            m.ApplyForce(force);

            //Each oscillator object oscillating on the x-axis
            float x = Random.Range(-10f, 10f) + Mathf.Cos(m.angle.x) * m.amplitude.x;

            //Each oscillator object oscillating on the y-axis
            float y = Random.Range(-5f, 5f) + Mathf.Cos(m.angle.y) * m.amplitude.y;

            //Add the oscillator's velocity to its angle
            m.angle += m.body.velocity;

            // Draw the line for each oscillator
            m.lineRender.SetPosition(0, l_attractors[attractorIndex].attractor.transform.position);
            m.lineRender.SetPosition(1, m.body.transform.position);

            //Move the oscillator
            m.body.transform.transform.Translate(new Vector2(x, y) * Time.deltaTime);

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
                    Vector2 attractedMover = movers[j].Repel(movers[i].body);

                    // We then apply that force the mover with the Rigidbody's Addforce() method
                    movers[i].body.AddForce(attractedMover * 0.08f, ForceMode.Impulse);
                }
            }
            // Now we check the boundaries of our scene to make sure the movers don't fly off
            // When we use gravity, the Movers will naturally fall out of the camera's view
            // This stops that.
            movers[i].CalculatePosition();
        }
    }
}


public class Attractor2_Step3_2
{
    public Vector3 angle, amplitude;
    private float radius;
    private float mass;
    private float G;

    private Vector2 location;

    public Rigidbody body;
    public GameObject attractor;

    public Vector2 maximumPos;

    public Attractor2_Step3_2()
    {
        FindWindowLimits();

        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(attractor.GetComponent<SphereCollider>());
        Renderer renderer = attractor.GetComponent<Renderer>();

        body = attractor.AddComponent<Rigidbody>();
        amplitude = new Vector2(Random.Range(-maximumPos.x / 2, maximumPos.x / 2), Random.Range(-maximumPos.y / 2, maximumPos.y / 2));
        body.position = new Vector3(Random.Range(-20f, 20f), Random.Range(-10f, 10f), 0f);
        body.velocity = new Vector2(Random.Range(-.05f, .05f), Random.Range(-0.05f, 0.05f));
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
        float strength = (G * body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }


    public void CheckEdges()
    {
        location = attractor.transform.position;

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

        attractor.transform.position = location;
    }


    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 15;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

public class Mover3_Step3_1
{
    public Vector3 angle, amplitude, center;
    // The basic properties of a mover class
    public Rigidbody body;
    private Transform transform;
    private GameObject mover;

    private Vector2 maximumPos;
    public LineRenderer lineRender;

    public Mover3_Step3_1(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
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

        // Create a GameObject that will be the line
        GameObject lineDrawing = new GameObject();

        // Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));
        amplitude = new Vector2(Random.Range(-maximumPos.x / 2, maximumPos.x / 2), Random.Range(-maximumPos.y / 2, maximumPos.y / 2));


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

    public Vector2 Repel(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (9.81f * body.mass * m.mass) / (distance * distance);
        force *= -strength;
        return force;
    }

    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 15;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}


