﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionClient : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}