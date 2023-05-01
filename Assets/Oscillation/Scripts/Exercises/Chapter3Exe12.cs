using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe12 : MonoBehaviour
{
    // Get the gravity strength and the pendulum from the scene
    [SerializeField] float gravity;
    [SerializeField] float damping;
    [SerializeField] float radius;
    [SerializeField] float startingAngleDegrees;


    [SerializeField] float damping2;
    [SerializeField] float radius2;
    [SerializeField] float startingAngleDegrees2;


    //Pendulum pendulum;
    [SerializeField] GameObject goPendulum;
    [SerializeField] GameObject goPendulum2;

    PendulumExe12 pendulum;
    PendulumExe12 pendulum2;


    Vector2 maximumPos;

    void Start()
    {
        FindWindowLimits();
        pendulum = goPendulum.GetComponent<PendulumExe12>();
        pendulum.gravity = gravity;
        pendulum.damping = damping;
        pendulum.radius = radius;
        pendulum.angle = (180 - startingAngleDegrees) * Mathf.Deg2Rad;



        pendulum2 = goPendulum2.GetComponent<PendulumExe12>();
        pendulum2.pivot = pendulum.getBobPosition();
        pendulum2.gravity = gravity;
        pendulum2.damping = damping2;
        pendulum2.radius = radius2;
        pendulum2.angle = (180 - startingAngleDegrees2) * Mathf.Deg2Rad;

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
