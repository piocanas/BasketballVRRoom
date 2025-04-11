using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenshot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScreenCapture.CaptureScreenshot("ShotArcScreenshot.png");
    }

    // Update is called once per frame
    void Update()
    {
        ScreenCapture.CaptureScreenshot("ShotArcScreenshot.png");
    }

}
