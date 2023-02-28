using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chapter1Exe3 : MonoBehaviour
{
    // Variables for the location and speed of mover
    private Vector3 location;
    private Vector3 speed = new Vector3(0.1f, 0.1f, 0.1f);
    

    // Variables to limit the mover within the screen space
    private Vector3 maximumPos;
    private float xMin, yMin, zMin, xMax, yMax, zMax;

    // A Variable to represent our mover in the scene
    private GameObject mover;

    // Start is called before the first frame update
    void Start()
    {
        // Call FindWindowLimits() on start
        FindWindowLimits();

        // We can now properly assign the Min and Max for the scene
        xMin = -maximumPos.x;
        xMax = maximumPos.x;
        yMin = -maximumPos.y;
        yMax = maximumPos.y;
        zMin= -maximumPos.z;
        zMax = maximumPos.z;

        // We now can set the mover as a primitive sphere in unity
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        // Each frame, we will check to see if the mover has touched a border
        // We check if the X/Y position is greater than the max position OR if it's less than the minimum position
        bool xHitBorder = location.x > xMax || location.x < xMin;
        bool yHitBorder = location.y > yMax || location.y < yMin;
        bool zHitBorder = location.z > zMax || location.z < zMin;

        // If the mover has hit at all, we will mirror it's speed with the corrisponding boarder
        if (xHitBorder)
        {
            speed.x = -speed.x;
        }
        if (yHitBorder)
        {
            speed.y = -speed.y;
        }
        if (zHitBorder)
        {
            speed.z = -speed.z;
        }

        // Lets now update the location of the mover
        location.x += speed.x;
        location.y += speed.y;
        location.z += speed.z;

        // Now we apply the positions to the mover to put it in it's place
        mover.transform.position = location;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = false;
        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0f, 10f, -20f);
        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 25));
    }

}

