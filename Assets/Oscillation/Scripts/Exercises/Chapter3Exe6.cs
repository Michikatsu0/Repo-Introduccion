using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe6 : MonoBehaviour
{
    [SerializeField] float period = 5f;
    [SerializeField] private float amplitude = 5f;

    private GameObject sphere;
    private MeshRenderer sphereRenderer;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // Set the camera to orthographic with a size of 5
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5;

        // Create a GameObject that will hold the LineRenderer component
        lineDrawing = new GameObject();

        // Create a sphere to oscillate
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //We need to create a new material for WebGL
        sphereRenderer = sphere.GetComponent<MeshRenderer>();
        sphereRenderer.material = new Material(Shader.Find("Diffuse"));

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a black line.
        lineRender = lineDrawing.AddComponent<LineRenderer>();

        //Make the line smaller for aesthetics
        lineRender.startWidth = 0.1f;
        lineRender.endWidth = 0.1f;

        //We need to create a new material for WebGL
        lineRender.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        var temp = ((Mathf.Sin((Mathf.PI) * Time.time / period)) * 0.5f);
        temp = temp + 0.5f;

        float x = Mathf.Lerp(-3, amplitude, Mathf.InverseLerp(0, 1, temp));

        sphere.transform.position = new Vector2(0, -x);

        //Begin rendering the line between the two objects. Set the first point (0) at the center Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector2 center = new Vector2(0f, 5f);
        lineRender.SetPosition(0, center);
        lineRender.SetPosition(1, sphere.transform.position);

    }
}
