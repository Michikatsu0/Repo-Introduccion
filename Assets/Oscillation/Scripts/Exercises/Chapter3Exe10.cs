using System.Collections.Generic;
using UnityEngine;

public class Chapter3Exe10 : MonoBehaviour
{ 
    [SerializeField] float waveSpeed1 = 1; 
    [SerializeField] float period1 = 10; 
    [SerializeField] float amplitude1 = 0.5f; 
    [SerializeField] GameObject waverPrefab1; 
    [SerializeField] int amountWavers1; 
    [SerializeField] private float offset1;

    [SerializeField] float waveSpeed2 = 1;
    [SerializeField] float period2 = 10;
    [SerializeField] float amplitude2 = 0.5f;
    [SerializeField] GameObject waverPrefab2;
    [SerializeField] int amountWavers2;
    [SerializeField] private float offset2;


    private Vector2 maximumPos;
    private Waves wave1;
    private Waves wave2;


    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();

        wave1 = new Waves(waveSpeed1, period1, amplitude1, waverPrefab1, amountWavers1, offset1, maximumPos);
        wave2 = new Waves(waveSpeed2, period2, amplitude2, waverPrefab2, amountWavers2, offset2, maximumPos);

    }

    // Update is called once per frame
    void Update()
    {
        wave1.Update();
        wave2.Update();
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

public class Waves
{

    // Campos que varian para cada objeto
    private float waveSpeed;
    private float period;
    private float amplitude;
    GameObject waverPrefab;
    int amountWavers;
    private float offset;
    private float startAngle = 0f;
    private List<Transform> waveTransforms;
    private Vector2 maximumPos;

    // método para crear las partículas de cada wave
    // E inicializar los campos.
    // sería el constructor de la clase

    public Waves(float _waveSpeed, float _period, float _amplitude, GameObject _waverPrefab, int _amountWavers, float _offset, Vector2 _maximumPos)
    {

        waveSpeed = _waveSpeed;
        period = _period;
        amplitude = _amplitude;
        waverPrefab = _waverPrefab;
        amountWavers = _amountWavers;
        offset = _offset;
        maximumPos = _maximumPos;

        // Populate the wave objects
        waveTransforms = new List<Transform>();
        while (waveTransforms.Count < amountWavers)
        {
            GameObject newWaver = Object.Instantiate(waverPrefab);
            waveTransforms.Add(newWaver.transform);
        }
    }


    public void Update()
    {
        startAngle += waveSpeed * Time.deltaTime;

        // Calculate the position of each object in the wave
        float currentAngle = startAngle;
        float currentX = -maximumPos.x;
        foreach (Transform waveTransform in waveTransforms)
        {
            // Step along the circle, a larger period means steps are smaller
            currentAngle += 1 / period;

            // Remap the sin function so that y(-1, 1) corresponds to y(bottom, top)
            float currentY = Mathf.Lerp(-maximumPos.y, maximumPos.y, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(currentAngle)));

            // Set the position of each wave transform
            waveTransform.position = new Vector2(currentX, (currentY * amplitude) + offset);

            // Step along the screen width such that every waver is on screen
            currentX += (maximumPos.x - -maximumPos.x) / amountWavers;
        }

    }
}