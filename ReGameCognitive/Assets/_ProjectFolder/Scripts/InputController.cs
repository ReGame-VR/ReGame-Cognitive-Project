using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool spacebarTrigger = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacebarTrigger = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spacebarTrigger = false;
        }
    }
}
