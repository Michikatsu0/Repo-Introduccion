using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Instead of objects bouncing off the edge of the wall, create an example in which an invisible force pushes back on the objects to keep them in the window.
//Can you weight the force according to how far the object is from an edge—i.e., the closer it is, the stronger the force?
public class Chapter2Exe3 : MonoBehaviour
{
    // Geometry defined in the inspector. [SerializeField] makes the variable avaialable in the inspector
    [SerializeField] float topY;
    [SerializeField] float floorY;
    [SerializeField] float leftWallX;
    [SerializeField] float rightWallX;
    [SerializeField] float inv;
    [SerializeField] Transform moverSpawnTransform;
    // Create a list of movers
    private List<Mover2_E3> movers = new List<Mover2_E3>();

    private Vector3 gravity = new Vector3(0, -0.005f, 0f);
    private Vector3 wind = new Vector3(0.004f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        // Create copys of our mover and add them to our list
        while (movers.Count < 3)
        {
            // Instantiate the mover and add it to our list. Pass in the spawn location, x bounds of the walls and the y location of the floor
            movers.Add(new Mover2_E3(moverSpawnTransform.position, leftWallX, rightWallX, floorY,topY));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Mover2_E3 mover in movers)
        {
            // ForceMode.Impulse and Forcemode.Force take mass into account
            mover.body.AddForce(gravity, ForceMode.Force);
            mover.body.AddForce(wind, ForceMode.Force);

            mover.CheckEdges(inv);
        }
    }
}

public class Mover2_E3
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private float xSpawn;

    private float distance1, distance2, distance3, dictance4;
    private Vector3 vector1, vector2, vector3, vector4;
    public Mover2_E3 (Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();

        // Remove functionality that comes with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        // We are using the gravity force we defined above so we do not want to use built in gravity
        body.useGravity = false;

        // Generate random properties for this mover
        radius = Random.Range(0.1f, 0.4f);

        // Generate a random x value within the bundaries
        xSpawn = Random.Range(xMin, xMax);

        // Place our mover at a randomized spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = new Vector3(xSpawn, position.y, position.z) + Vector3.up * radius;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * (radius * radius * radius);
        
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckEdges(float inv)
    {
        distance1 = yMin - body.transform.position.y;
        distance2 = xMin - body.transform.position.x;
        distance3 = xMax - body.transform.position.x;
        dictance4 = yMax - body.transform.position.y;

        vector1.y = inv / Mathf.Pow(distance1, 2);
        vector2.x = inv / Mathf.Pow(distance2, 2);
        vector3.x = (inv / Mathf.Pow(distance3, 2)) * -1;
        vector4.y = (inv / Mathf.Pow(dictance4, 2)) * -1;


        Vector3 restrainedVelocity = body.velocity;


        body.AddForce(vector2, ForceMode.Impulse);
        body.AddForce(vector4, ForceMode.Impulse);
        body.AddForce(vector1, ForceMode.Impulse);
        body.AddForce(vector3, ForceMode.Impulse);

        if (body.position.y - radius < yMin)
        {
            
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
            body.position = new Vector3(body.position.x, yMin, body.position.z) + Vector3.up * radius;
        }
        if (body.position.y + radius > yMax)
        {
            
            restrainedVelocity.y = -Mathf.Abs(restrainedVelocity.y);
            body.position = new Vector3(body.position.x, yMin, body.position.z) + Vector3.down * radius;
        }
        if (body.position.x - radius < xMin)
        {
            
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
            body.position = new Vector3(xMin, body.position.y, body.position.z) + Vector3.right * radius;
        }
        else if (body.position.x + radius > xMax)
        {
            
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);
            body.position = new Vector3(xMax, body.position.y, body.position.z) + Vector3.left * radius;
        }
        body.velocity = restrainedVelocity;
    }
}
