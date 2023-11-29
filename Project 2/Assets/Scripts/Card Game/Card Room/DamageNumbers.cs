using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    private float destroyTime = 3f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.up / 1000;
    }
}
