using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnStart : MonoBehaviour
{
    public float scaleZ = 0.001f;

    void Update()
    {
        Vector3 newScale = transform.localScale;
        newScale.z = scaleZ;
        transform.localScale = newScale;

       
    }
}
