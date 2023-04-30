using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig10 : MonoBehaviour
{
  
}

public class Pendulum
{

    Vector3 location;
    public Vector2 origin;

    public float radius;
    public float angle;
    public float aVelocity = 0.0f;
    public float aAcceleration = 0.0f;
    public float damping = 0.995f;
    GameObject lineDrawing;
    LineRenderer lineRender;
}