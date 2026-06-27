using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CursorController : MonoBehaviour
{
    public bool showCursor;

    void Start()
    {
        SetCursorActive(false);
    }

    void Update()
    {
        //Cursor.visible = showCursor;
    }
    public void SetCursorActive(bool value)
    {
        showCursor = value;

        Cursor.visible = showCursor;
        
        if (showCursor){ Cursor.lockState = CursorLockMode.None; }
        else{ Cursor.lockState = CursorLockMode.Confined; }
    }
}
