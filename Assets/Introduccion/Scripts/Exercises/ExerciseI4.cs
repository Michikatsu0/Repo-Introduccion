using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseI4 : MonoBehaviour
{
    public float minRadius = 0.5f;
    public float maxRadius = 2.5f;
    // Update is called once per frame
    void FixedUpdate()
    {


        // To create a Gaussian distribution in Unity we can use Random.Range() within two separate Random.Range()
        float numX = Random.Range(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        float numY = Random.Range(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        float sd = 30;
        float mean = 5;

        float x = sd * numX + mean;
        float y = sd * numY + mean;


        // This creates a sphere GameObject and applies the transparency material prefab we created in Unity.
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer r = sphere.GetComponent<Renderer>();
        r.material.color = Random.ColorHSV(Random.Range(0f, 1f), Random.Range(0f, 1f));
        // This instantiates the sphere with an X position calculated by our Gaussian distribution
        float radius = Mathf.Clamp(minRadius + maxRadius * Random.Range(-1f, 1f), minRadius, maxRadius);
        sphere.transform.position = new Vector3(x, y, 0F)* radius * Time.deltaTime;

    }
}