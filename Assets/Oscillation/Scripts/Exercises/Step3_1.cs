using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step3_1 : MonoBehaviour
{
    List<Oscillator_Step3_1> oscilattors = new List<Oscillator_Step3_1>();
    List<Mover2_Step2_2> movers = new List<Mover2_Step2_2>();

    Attractor2_Step2_2 attractor;
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
        while (oscilattors.Count < 10)
        {
            oscilattors.Add(new Oscillator_Step3_1());

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Oscillator_Step3_1 o in oscilattors)
        {
            //Each oscillator object oscillating on the x-axis
            float x = Mathf.Cos(o.angle.x) * o.amplitude.x;

            //Each oscillator object oscillating on the y-axis
            float y = Mathf.Cos(o.angle.y) * o.amplitude.y;

            //Add the oscillator's velocity to its angle
            o.angle += o.velocity;

            // Draw the line for each oscillator
            o.lineRender.SetPosition(1, o.oGameObject.transform.position);

            //Move the oscillator
            o.oGameObject.transform.transform.Translate(new Vector2(x, y) * Time.deltaTime);
        }
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

public class Oscillator_Step3_1
{

    // The basic properties of an oscillator class
    public Vector2 velocity, angle, amplitude, center;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject oGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    // Create variables for rendering the line between two vectors
    public LineRenderer lineRender;

    public Oscillator_Step3_1()
    {
        FindWindowLimits();

        // Initialize the angle at 0, 0
        angle = Vector2.zero;

        // Generate a random velocity and amplitude for each oscillator
        velocity = new Vector2(Random.Range(-.05f, .05f), Random.Range(-0.05f, 0.05f));
        amplitude = new Vector2(Random.Range(-maximumPos.x / 2, maximumPos.x / 2), Random.Range(-maximumPos.y / 2, maximumPos.y / 2));

        // Create a new material for WebGL
        Renderer r = oGameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        // Create a GameObject that will be the line
        GameObject lineDrawing = new GameObject();

        // Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));

        // Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        // Make sure the end of the line (1) appears at the new Vector3 in FixedUpdate()
        lineRender.SetPosition(0, center);
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // For correct functionality, we set the camera x and y position to 0, 0
        Camera.main.transform.position = new Vector3(0, 0, -10);
        // Next we grab the minimum and maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}