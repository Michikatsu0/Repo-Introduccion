
using UnityEngine;

public class Step1_1 : MonoBehaviour
{
    // Create an array of 10 movers
    private Mover1_Step1_1[] movers = new Mover1_Step1_1[10];

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate each mover in the array as a new mover
        for (int i = 0; i < movers.Length; i++)
        {
            movers[i] = new Mover1_Step1_1();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        for (int i = 0; i < movers.Length; i++)
        {
            Vector2 RandomPointsInWindowLimits = new Vector2(Random.Range(-movers[i].maximumPos.x, movers[i].maximumPos.x), Random.Range(-movers[i].maximumPos.y, movers[i].maximumPos.y));
            Vector2 dir = movers[i].SubtractVectors(RandomPointsInWindowLimits, movers[i].location);
            movers[i].acceleration = movers[i].ScaleVector(dir.normalized, Random.Range(2f,15f));
            movers[i].Step();
            movers[i].CheckEdges();
        }
    }
}

public class Mover1_Step1_1
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    public Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_Step1_1()
    {
        FindWindowLimits();

        location = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));

        // Vector2.zero is shorthand for a (0, 0) vector
        velocity = Vector2.zero;
        acceleration = Vector2.zero;

        // Set top speed to 1f
        topSpeed = 1f;

        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Step()
    {
        if (velocity.magnitude <= topSpeed)
        {
            // Speeds up the mover
            velocity += acceleration * Time.deltaTime;

            // Limit Velocity to the top speed
            velocity = Vector2.ClampMagnitude(velocity, topSpeed);

            // Moves the mover
            location += velocity * Time.deltaTime;

            // Updates the GameObject of this movement
            mover.transform.position = new Vector3(location.x, location.y, 0);
        }
        else
        {
            velocity -= acceleration * Time.deltaTime;
            location += velocity * Time.deltaTime;
            mover.transform.position = new Vector3(location.x, location.y, 0);
        }
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
            ResetMovementVariables();
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
            ResetMovementVariables();
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
            ResetMovementVariables();
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
            ResetMovementVariables();
        }
    }

    private void ResetMovementVariables()
    {
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0 for x and y. We will use -10 for z in this example
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    // This method calculates A - B component wise
    // SubtractVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA - vecB
    public Vector2 SubtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates a vector scaled by a factor component wise
    // ScaleVector(vector, factor) will yield the same output as Unity's built in operator: vector * factor
    public Vector2 ScaleVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }

}