using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChengeRT : MonoBehaviour
{
    
        void Update()
        {
            SetScaleZToZero(transform);
        }

        void SetScaleZToZero(Transform parentTransform)
        {
        if (parentTransform != null)
        {
            Vector3 newPosition = parentTransform.localPosition;
            newPosition.z = 0f;
            parentTransform.localPosition = newPosition;
        }

        foreach (Transform child in parentTransform)
        {
            SetScaleZToZero(child);
        }
    }

    
}
