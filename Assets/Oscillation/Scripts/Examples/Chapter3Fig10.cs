using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig10 : MonoBehaviour
{
    // Get the gravity strength and the pendulum from the scene
    [SerializeField] float gravity;
    [SerializeField] float damping;
    [SerializeField] float radius;
    [SerializeField] float startingAngleDegrees;

    PendulumFig10 pendulum;
    Vector2 maximumPos;

    void Start()
    {
        FindWindowLimits();

        // Create a new instance of the Pendulum behavior
        pendulum = gameObject.AddComponent<PendulumFig10>();
        // Position the pivot for the pendulum at the top center of the camera
        pendulum.pivot = new Vector2(0, maximumPos.y);
        // Pass the values we assigned into the new pendulum
        pendulum.gravity = gravity;
        pendulum.damping = damping;
        pendulum.radius = radius;

        // Adjust the angle since 0 degrees should point down, not up
        pendulum.angle = (180 - startingAngleDegrees) * Mathf.Deg2Rad;
    }

    private void FindWindowLimits()
    {
        // Start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // For correct functionality, we set the camera x and y position to 0, 0
        Camera.main.transform.position = new Vector3(0, 0, -10);
        // Next we grab the minimum and maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}


public class PendulumFig10 : MonoBehaviour
{
    GameObject gameObjectHeight;
    // Start is called before the first frame update
    public Vector3 pivot = new Vector3(0f, 10f, 0f);
    public float radius = 1;
    public float angle = 0;
    public float damping = 1;
    public float gravity = 0;

    // Other scripts cannot see private variables
    LineRenderer lineRenderer;
    float aVelocity;
    float aAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the pendulum in a resting state
        aVelocity = 0;
        aAcceleration = 0;

        // Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Diffuse"));
        lineRenderer.startWidth = .5f;
        lineRenderer.endWidth = .5f;

        // Create a sphere for the bob of the pendulum
        gameObjectHeight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(gameObjectHeight.GetComponent<SphereCollider>());

        // Support for WebGL
        Renderer r = gameObjectHeight.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));


    }

    // Update is called once per frame
    public void Update()
    {
        aAcceleration = (-gravity / radius) * Mathf.Sin(angle);
        aVelocity += aAcceleration * Time.deltaTime;
        angle += aVelocity;

        aVelocity *= damping;


        this.gameObject.transform.position = new Vector3(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle), 0f);
        this.gameObject.transform.position += pivot;

        gameObjectHeight.transform.position = gameObject.transform.position;

        lineRenderer.SetPosition(0, pivot);
        lineRenderer.SetPosition(1, this.gameObject.transform.position);
    }
}