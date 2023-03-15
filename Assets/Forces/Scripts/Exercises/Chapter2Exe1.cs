using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exe1 : MonoBehaviour
{
    // Geometry defined in the inspector. [SerializeField] makes the variable avaialable in the inspector
    [SerializeField] float floorY;
    [SerializeField] float topY;
    [SerializeField] float leftWallX;
    [SerializeField] float rightWallX;
    [SerializeField] Transform moverSpawnTransform;

    Mover2_E1 mover;


    // Range over which height varies.
    [SerializeField] float heightScale = 10.0f;

    // Distance covered per second along X axis of Perlin plane.
    [SerializeField] float xScale = 0.1f;

    // Define constant forces in our environment

    public Vector3 wind = new Vector3(5f, 0f, 0f);

    private Vector3 helium = new Vector3(0f, 50f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the mover and pass in the spawn location, x bounds of the walls and the y location of the floor
        mover = new Mover2_E1(moverSpawnTransform.position, leftWallX, rightWallX, floorY, topY);
    }

    void CaclNoise()
    {
        float xCoord = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
        wind = new Vector3(xCoord, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply force to the mover, with ForceMode.Impulse taking mass into account

        mover.body.AddForce(helium, ForceMode.Force);
        mover.CheckEdges();
        CaclNoise();
        mover.body.AddForce(wind, ForceMode.Impulse);

    }
}

public class Mover2_E1
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Mover2_E1(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;

        // Initialize the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();

        // Remove functionality that come with the primitive that we don't want
        // Destroy() is a built in Unity function that removes a GameObject
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        // Generate a radius of 1f for this mover
        radius = 1f;

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * radius;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * (radius * radius * radius);
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckEdges()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            // Using the absolute value here is an important safe
            // guard for the scenario that it takes multiple ticks
            // of FixedUpdate for the mover to return to its boundaries.
            // The intuitive solution of flipping the velocity may result
            // in the mover not returning to the boundaries and flipping
            // direction on every tick.

            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
            body.position = new Vector3(body.position.x, yMin, body.position.z) + Vector3.up * radius;
        }
        if (body.position.y + radius > yMax)
        {
            // Using the absolute value here is an important safe
            // guard for the scenario that it takes multiple ticks
            // of FixedUpdate for the mover to return to its boundaries.
            // The intuitive solution of flipping the velocity may result
            // in the mover not returning to the boundaries and flipping
            // direction on every tick.

            restrainedVelocity.y = -Mathf.Abs(restrainedVelocity.y);
            body.position = new Vector3(body.position.x, yMax, body.position.z) + Vector3.down * radius;
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