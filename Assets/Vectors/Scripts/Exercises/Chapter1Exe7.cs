using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chapter1Exe7 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 v = new Vector2(1, 5);
        Vector2 u = VectorMaths.ScaleVector(v, 2);
        Vector2 w = VectorMaths.SubtractVectors(v, u);
        DivideVector(w, 3);

        Debug.Log(v);
        Debug.Log(u);
        Debug.Log(DivideVector(w, 3));
    }

    Vector2 DivideVector(Vector2 toDivide, float divideFactor)
    {
        float x = toDivide.x / divideFactor;
        float y = toDivide.y / divideFactor;
        return new Vector2(x, y);
    }
}

public static class VectorMaths
{
    public static Vector2 SubtractVectors(this Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates a vector scaled by a factor component wise
    // ScaleVector(vector, factor) will yield the same output as Unity's built in operator: vector * factor
    public static Vector2 ScaleVector(this Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
}
