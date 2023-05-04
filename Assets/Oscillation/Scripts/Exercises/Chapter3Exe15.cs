using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chapter3Exe15 : MonoBehaviour
{
    public Transform centerPoint;
    public float radiusX, radiusY, angularVelocity;
    [SerializeField] float springConstantK1 = 3.5f;
    [SerializeField] float restLength1 = 3f;
    [SerializeField] float springConstantK2 = 3.5f;
    [SerializeField] float restLength2 = 3f;
    [SerializeField] float springConstantK3 = 3.5f;
    [SerializeField] float restLength3 = 3f;
    [SerializeField] GameObject anchorTransform;
    [SerializeField] List<Rigidbody> bobBodys;
    [SerializeField] List<GameObject> springs3_15;
    // Start is called before the first frame update
    void Start()
    {
        // Add a new spring at the start of runtime

        Spring3_15 spring = springs3_15[0].AddComponent<Spring3_15>();

        spring.anchor = anchorTransform.transform;
        spring.connectedBody = bobBodys[0];
        spring.restLength = restLength1;
        spring.springConstantK = springConstantK1;
        // Add the click-drag behavior
        ClickDragBody3_15 mouseDrag = bobBodys[0].gameObject.AddComponent<ClickDragBody3_15>();
        mouseDrag.body = bobBodys[0];
        mouseDrag.radius = 4;

        Spring3_15 spring2 = springs3_15[1].AddComponent<Spring3_15>();

        spring2.anchor = bobBodys[0].transform;
        spring2.connectedBody = bobBodys[1];
        spring2.restLength = restLength2;
        spring2.springConstantK = springConstantK2;
        ClickDragBody3_15 mouseDrag2 = bobBodys[1].gameObject.AddComponent<ClickDragBody3_15>();
        mouseDrag2.body = bobBodys[1];
        mouseDrag2.radius =4;

        Spring3_15 spring3 = springs3_15[2].AddComponent<Spring3_15>();

        spring3.anchor = bobBodys[1].transform;
        spring3.connectedBody = bobBodys[2];
        spring3.restLength = restLength3;
        spring3.springConstantK = springConstantK3;

        ClickDragBody3_15 mouseDrag3 = bobBodys[2].gameObject.AddComponent<ClickDragBody3_15>();
        mouseDrag3.body = bobBodys[2];
        mouseDrag3.radius = 4;

        PivotMove pivotMove = anchorTransform.AddComponent<PivotMove>();
        pivotMove.angularVelocity = angularVelocity;
        pivotMove.radiusX = radiusX;
        pivotMove.radiusY = radiusY;
        pivotMove.centerPoint = centerPoint;
        pivotMove.anchor = anchorTransform.transform;
    }
}

public class Spring3_15 : MonoBehaviour
{
    // Properties that need to be assigned by the inspector or other scripts
    public Transform anchor;
    public Rigidbody connectedBody;
    public float restLength = 1;
    public float springConstantK = 0.1f;
    LineRenderer lineRenderer;

    void Start()
    {
        // Instantiate LineRenderer, add a material and scale the width
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Diffuse"));
        lineRenderer.widthMultiplier = 0.5f;
    }

    void Update()
    {
        // Get the difference Vector3 between the anchor and the body
        Vector3 force = connectedBody.position - anchor.position;

        // Find the distance/magnitude of the vector using .magnitude
        float currentLength = force.magnitude;
        float stretchLength = currentLength - restLength;

        // Reverse the direction of the force arrow and set its length
        // based on the spring constant and stretch
        force = -force.normalized * springConstantK * stretchLength;

        // Apply the force to the connected body relative to time
        connectedBody.AddForce(force * Time.deltaTime, ForceMode.Impulse);

        // Draw the line along the spring
        lineRenderer.SetPosition(0, anchor.position);
        lineRenderer.SetPosition(1, connectedBody.position);
    }
}

public class ClickDragBody3_15 : MonoBehaviour
{
    public Rigidbody body;
    public float radius;

    Renderer bodyRenderer;

    Material defaultMaterial;
    Material mouseOverMaterial;

    bool isDragging = false;

    void Start()
    {
        bodyRenderer = body.gameObject.GetComponent<Renderer>();
        defaultMaterial = bodyRenderer.material;

        // WebGL Support
        mouseOverMaterial = new Material(Shader.Find("Diffuse"));
        mouseOverMaterial.color = Color.green;
    }

    void Update()
    {
        DragBob();
    }

    void DragBob()
    {
        // Declare a Vector2 for the location of the mouse in world space
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Set the renderer material to default
        bodyRenderer.material = defaultMaterial;

        // If the mouse location is not within the radius of the body, return
        if (Vector2.Distance(mouseLocation, body.position) > radius)
            return;

        // If the mouse location is within the radius of the body, set material to mousover material
        bodyRenderer.material = mouseOverMaterial;

        // If the mouse location is within the radius of the body and the mouse is clicked, set isDragging to true
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        // If isDragging is true, lock the position of the body to the mouse position until the mouse button is released
        if (isDragging)
        {
            body.position = mouseLocation;
            if (Input.GetMouseButtonUp(0))
            {
                body.position = mouseLocation;
                body.velocity = Vector3.zero;
                isDragging = false;
            }
        }

    }
}

public class PivotMove : MonoBehaviour
{
    public Transform anchor;
    public Transform centerPoint;
    public float radiusX, radiusY, angularVelocity;
    Vector2 position;
    float angle;
    private void Update()
    {
        position.x = centerPoint.position.x + Mathf.Cos(angularVelocity * angle) * radiusX;
        position.y = centerPoint.position.y + Mathf.Sin(angularVelocity * angle) * radiusY;
        angle += Time.deltaTime;

        if (angle >= 360)
            angle = 0;

        anchor.position = position;
    }
}