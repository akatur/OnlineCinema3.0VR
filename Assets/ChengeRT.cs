using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChengeRT : MonoBehaviour
{

    public float pozZ = 1.2f;
    //public float pozy = 1.2f;


        void Update()
        {
            SetScaleZToZero(transform);
        }

        void SetScaleZToZero(Transform parentTransform)
        {
        if (parentTransform != null)
        {
            Vector3 newPosition = parentTransform.localPosition;
            newPosition.z = pozZ;
            //newPosition.y = pozy;
            parentTransform.localPosition = newPosition;
        }

        foreach (Transform child in parentTransform)
        {
            SetScaleZToZero(child);
        }
    }

    
}
