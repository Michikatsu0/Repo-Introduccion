using UnityEngine;

public class Exercise2 : MonoBehaviour
{
    // We need to create a mover instance
    IntroMoverEI2 mover;
    // Start is called before the first frame update
    void Start()
    {
        mover = new IntroMoverEI2();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Have the mover step and check edges
        mover.Step();
        mover.CheckEdges();
    }
}


public class IntroMoverEI2
{
    private Transform targetPos;
    // The basic properties of a mover class
    // x, y, z 
    private Vector3 location;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public IntroMoverEI2()
    {
        

        FindWindowLimits();
        // Set location to Vector2.zero, shorthand for (0, 0)
        location = Vector2.zero;
        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Step()
    {
        this.targetPos = GameObject.Find("target1").transform;

        location = mover.transform.position;
        float choice = Random.Range(0f, 5f);

        if (choice < 0.2f)
        {
            mover.transform.position = Vector3.Lerp(location, targetPos.position, 0.08f);
        }
        else if (choice > 0.2f && choice < 0.5f)
        {
            location.x++;
        }
        else if (choice > 0.5f && choice < 0.7f)
        {
            location.x--;
        }
        else if (choice > 0.7f && choice < 0.9f)
        {
            location.y++;
        }
        else if (choice > 0.9f && choice < 1)
        {
            location.y--;
        }
        mover.transform.position += location * Time.deltaTime;
    }

    public void CheckEdges()
    {
        location = mover.transform.position;
        // Reset location to zero upon reaching maximum or(||) negative maximum(minimum)
        if (location.x > maximumPos.x || location.x < -maximumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y || location.y < -maximumPos.y)
        {
            location = Vector2.zero;
        }
        // Set the position of the mover to location
        mover.transform.position = location;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // The maximum position can be attributed to the minimum bounds by setting it negative
        // maximumPos and -maximumPos equate to maximum and minimum screen bounds
    }
}