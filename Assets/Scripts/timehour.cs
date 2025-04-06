using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timehour : MonoBehaviour
{
    // Start is called before the first frame update
    private float initialY;
    private float initialZ;
    void Start()
    {
        DateTime localDate = DateTime.Now;
        initialY = transform.rotation.eulerAngles.y;
        initialZ = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current hour
        int hour = DateTime.Now.Hour % 12; // Convert 24-hour format to 12-hour format

        // Calculate the rotation angle (each hour is 30 degrees)
        float hourRotation = hour * 30f;

        // Apply rotation to the hour hand
        transform.rotation = Quaternion.Euler(hourRotation, initialY, initialZ);
    }
}
