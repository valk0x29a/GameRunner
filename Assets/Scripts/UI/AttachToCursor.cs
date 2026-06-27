using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AttachToCursor : MonoBehaviour
{
    RectTransform attach;
    void Start()
    {
        attach = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 cursorPosition = InputSystem.actions.FindActionMap("Movement").FindAction("Mouse").ReadValue<Vector2>();
        attach.position = cursorPosition;
    }
}
