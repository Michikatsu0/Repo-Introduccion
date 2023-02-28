using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Exe1 : MonoBehaviour
{
    // We need to create a mover instance
    Mover1_E1 mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = new Mover1_E1();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Have the mover step and check edges
        mover.Step();
        mover.CheckEdges();
    }
}


public class Mover1_E1
{
    // The basic properties of a mover class
    // x, y, z 
    private Vector3 location;
    //public float xstep;
    //public float ystep;

    // The window limits
    private Vector2 maximumPos;
    public Vector2 movement;
    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_E1()
    {
        FindWindowLimits();
        // Set location to Vector2.zero, shorthand for (0, 0)
        location = Vector2.zero;
        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Step()
    {
        location = mover.transform.position;

        float r = Random.Range(0f, 1f);
        if (r < 0.05)
        {
            //xstep = Random.Range(-100f, 100f);
            //ystep = Random.Range(-100f, 100f);
            movement = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
        }
        else
        {
            //xstep = Random.Range(-2F, 2f);
            //ystep = Random.Range(-2f, 2f);
            movement = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        }
        location = movement;


        mover.transform.position += location * Time.deltaTime;
    }

    public void CheckEdges()
    {
        location = mover.transform.position;
        // Reset location to zero upon reaching maximum or(||) negative maximum(minimum)
        if (location.x > maximumPos.x || location.x < -maximumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y || location.y < -maximumPos.y)
        {
            location = Vector2.zero;
        }
        // Set the position of the mover to location
        mover.transform.position = location;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // The maximum position can be attributed to the minimum bounds by setting it negative
        // maximumPos and -maximumPos equate to maximum and minimum screen bounds
    }
}