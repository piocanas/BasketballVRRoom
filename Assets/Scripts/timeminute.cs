using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class timeminute : MonoBehaviour
{
    // Start is called before the first frame update
    private float initialY;
    private float initialZ;
    void Start()
    {
        initialY = transform.rotation.eulerAngles.y;
        initialZ = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        int minute = DateTime.Now.Minute;

        float minuteRotation = minute * 6f;

        transform.rotation = Quaternion.Euler(minuteRotation, initialY, initialZ);
    }
}
