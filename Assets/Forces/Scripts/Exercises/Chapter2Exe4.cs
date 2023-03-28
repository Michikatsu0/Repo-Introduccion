
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exe4 : MonoBehaviour
{
    // Geometry defined in the inspector.
    [SerializeField] float floorY;
    [SerializeField] float leftWallX;
    [SerializeField] float rightWallX;
    [SerializeField] Transform moverSpawnTransform;

    [SerializeField] Transform fluidCorner1A;
    [SerializeField] Transform fluidCorner1B;
    [SerializeField] Transform fluidCorner2A;
    [SerializeField] Transform fluidCorner2B;
    [SerializeField] Transform fluidCorner3A;
    [SerializeField] Transform fluidCorner3B;
    [SerializeField] Material waterMaterial;


    // Create a list of movers
    private List<Mover2_E4> movers = new List<Mover2_E4>();
    private List<Pocket> pockets = new List<Pocket>();

    // Define constant forces in our environment
    private Vector3 wind = new Vector3(0f, 0f, 0f);

    [SerializeField] private float frictionStrength1 = 0.5f;
    [SerializeField] private float frictionStrength2 = -0.5f;
    [SerializeField] private float frictionStrength3 = 4.5f;

    // Start is called before the first frame updateS
    void Start()
    {
        // Create copies of our mover and add them to our list
        while (movers.Count < 30)
        {
            movers.Add(new Mover2_E4(moverSpawnTransform.position, leftWallX, rightWallX, floorY));
        }

        pockets.Add(new Pocket(fluidCorner1A.position, fluidCorner1B.position, frictionStrength1, waterMaterial));
        pockets.Add(new Pocket(fluidCorner2A.position, fluidCorner2B.position, frictionStrength2, waterMaterial));
        pockets.Add(new Pocket(fluidCorner3A.position, fluidCorner3B.position, frictionStrength3, waterMaterial));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Mover2_E4 mover in movers)
        {
            // ForceMode.Impulse takes mass into account
            mover.body.AddForce(wind, ForceMode.Impulse);
            foreach (Pocket pocket in pockets)
            {
                if (mover.IsInside(pocket))
                {
                    // Apply a friction force that directly opposes the current motion
                    Vector3 friction = -mover.body.velocity;
                    friction.Normalize();
                    friction *= pocket.frictionCoefficient;
                    mover.body.AddForce(friction, ForceMode.Force);
                }
            }
            mover.CheckEdges();
        }
    }
}

public class Mover2_E4
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float xSpawn;

    public Mover2_E4(Vector3 position, float xMin, float xMax, float yMin)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;

        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();

        // Remove functionality that comes with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

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
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
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
    public bool IsInside(Pocket pocket)
    {
        // Check to see if the mover is inside the range on each axis.
        if (body.position.x > pocket.minBoundary.x &&
            body.position.x < pocket.maxBoundary.x &&
            body.position.y > pocket.minBoundary.y &&
            body.position.y < pocket.maxBoundary.y &&
            body.position.z > pocket.minBoundary.z &&
            body.position.z < pocket.maxBoundary.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

public class Pocket
{
    public Vector3 minBoundary;
    public Vector3 maxBoundary;
    public float frictionCoefficient;

    public Pocket(Vector3 corner1, Vector3 corner2, float frictionCoefficient, Material material)
    {
        // Get the minimum and maximum corners of the rectangular prism
        // This code allows the designer to place the volume corners at
        // any of the eight possible diagonals of a rectangular prism.
        minBoundary = new Vector3(
            Mathf.Min(corner1.x, corner2.x),
            Mathf.Min(corner1.y, corner2.y),
            Mathf.Min(corner1.z, corner2.z)
        );
        maxBoundary = new Vector3(
            Mathf.Max(corner1.x, corner2.x),
            Mathf.Max(corner1.y, corner2.y),
            Mathf.Max(corner1.z, corner2.z)
        );
        this.frictionCoefficient = frictionCoefficient;

        // Create the presence of the object in 3D space
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.GetComponent<Renderer>().material = material;


        // Remove undesired components that come with the primitive
        obj.GetComponent<BoxCollider>().enabled = false;
        Object.Destroy(obj.GetComponent<BoxCollider>());

        // Position and scale the new cube to match the boundaries.
        obj.transform.position = (corner1 + corner2) / 2;
        obj.transform.localScale = new Vector3(
            Mathf.Abs(corner2.x - corner1.x),
            Mathf.Abs(corner2.y - corner1.y),
            Mathf.Abs(corner2.z - corner1.z)
        );
    }
}
