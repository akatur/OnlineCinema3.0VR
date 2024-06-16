using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField InputField;


    public float distance = 0.1f;
    public float verticaloffset = -1f;

    public Transform positionSource;

    private void Start()
    {
        InputField = GetComponent<TMP_InputField>();
        InputField.onSelect.AddListener(delegate { OpenKeyboard(); });
    }

    public void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = InputField;
        NonNativeKeyboard.Instance.PresentKeyboard(InputField.text);

        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticaloffset;

        NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
    }
}
