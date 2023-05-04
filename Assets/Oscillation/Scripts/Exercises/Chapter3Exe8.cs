using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Chapter3Exe8 : MonoBehaviour
{
    [SerializeField] private float period = 5f;
    [SerializeField] private float xVP, xVN, yVP, yVN;
    [SerializeField] private Vector2 amplitude;
    private List<Oscillator3_E8> oscilattors = new List<Oscillator3_E8>();

    void Start()
    {
        StartCoroutine(WaitForFunction());


    }

    IEnumerator WaitForFunction()
    {
        while (oscilattors.Count < 10)
        {
            oscilattors.Add(new Oscillator3_E8(period));
            yield return new WaitForSeconds(0.5f);

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        foreach (Oscillator3_E8 o in oscilattors)
        {
            //Each oscillator object oscillating on the x-axis
            o.amplitude = amplitude;
            var temp = ((Mathf.Sin((2 * Mathf.PI) * Time.deltaTime / period)));
            float x2 = Mathf.Lerp(xVN, xVP, Mathf.InverseLerp(1, 0, temp));

            var temp2 = ((Mathf.Cos((2 * Mathf.PI) * Time.deltaTime / period)));
            float y2 = Mathf.Lerp(yVN, yVP, Mathf.InverseLerp(1, 0, temp2));

            var velocity = new Vector2(x2, y2);

            o.velocity = velocity;

            // Generate a random velocity and amplitude for each oscillator
            float x = Mathf.Cos(o.angle.x) * o.amplitude.x;

            //Each oscillator object oscillating on the y-axis
            float y = Mathf.Cos(o.angle.y) * o.amplitude.y;

            //Add the oscillator's velocity to its angle
            o.angle += o.velocity;

            // Draw the line for each oscillator
            o.lineRender.SetPosition(1, o.oGameObject.transform.position);

            //Move the oscillator
            o.oGameObject.transform.transform.Translate(new Vector2(x, y) * Time.deltaTime/ velocity);
        }
    }
}

public class Oscillator3_E8
{

    // The basic properties of an oscillator class
    public Vector2 velocity, angle, amplitude;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject oGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    // Create variables for rendering the line between two vectors
    public LineRenderer lineRender;

    public Oscillator3_E8(float period)
    {
        FindWindowLimits();

        // Initialize the angle at 0, 0
        angle = Vector2.zero;
        amplitude = new Vector2(3, 3);

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
        Vector2 center = new Vector2(0f, 0f);
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