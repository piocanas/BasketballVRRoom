using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class spawnball : MonoBehaviour
{
    public GameObject ballPrefab; 
    public Transform handTransform;

    void Update()
    {
        // if (OVRInput.GetDown(OVRInput.RawButton.B))
        // {
            Spawn();
            gameObject.SetActive(false);
        //}
    }

    void Spawn()
    {
        Instantiate(ballPrefab, handTransform.position, handTransform.rotation);
    }
}
