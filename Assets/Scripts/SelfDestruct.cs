using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float timeTillDestroy = 3f;
    private void Start()
    {
        Destroy(gameObject, timeTillDestroy);
    }
}
