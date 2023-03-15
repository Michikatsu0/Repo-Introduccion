using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Exe2 : MonoBehaviour
{
    // Geometry defined in the inspector. [SerializeField] makes the variable avaialable in the inspector
    [SerializeField] float topY;
    [SerializeField] float floorY;
    [SerializeField] float leftWallX;
    [SerializeField] float rightWallX;
    [SerializeField] Transform moverSpawnTransform;
    [SerializeField] float delay;
    // Create a list of movers
    private Mover2_E2 mover;

    // Define constant forces in our environment
    private Vector3 wind = new Vector3(0.004f, 0f, 0f);
    private Vector3 gravity = new Vector3(0, -0.04f, 0f);
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        mover = new Mover2_E2(moverSpawnTransform.position, leftWallX, rightWallX, floorY, topY);
        time = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // Apply the forces to each of the Movers
        // ForceMode.Impulse and Forcemode.Force take mass into account
        mover.body.AddForce(wind, ForceMode.Impulse);
        mover.body.AddForce(gravity, ForceMode.Force);

        mover.CheckEdges();
        
        if (time > delay)
        {
            StartCoroutine(mover.ChanngeMass(delay));
            time = 0;
        }
        time += Time.deltaTime;

        
    }
}

public class Mover2_E2
{
    public Rigidbody body;
    private GameObject gameObject;
    public float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private float xSpawn;

    public Mover2_E2(Vector3 position, float xMin, float xMax, float yMin, float yMax)
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
        radius = 0.5f;

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

    public IEnumerator ChanngeMass(float delay)
    {
        radius = Random.Range(0.05f, 0.5f);
        
        yield return new WaitForSeconds(delay);
        body.mass = (4f / 3f) * Mathf.PI * (radius * radius * radius);
        //float xCoord = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
    }
}