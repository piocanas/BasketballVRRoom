using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform handTransform;
    public int maxBalls = 5;

    public Queue<GameObject> activeBalls = new Queue<GameObject>();

    void Update()
    {
        Spawn();
        gameObject.SetActive(false);
    }

     public void Spawn()
    {
        // If we reached the limit, remove (and destroy) the oldest one
        if (activeBalls.Count >= maxBalls)
        {
            GameObject oldBall = activeBalls.Dequeue();
            Destroy(oldBall);
        }

        // Instantiate new ball at hand position
        GameObject newBall = Instantiate(ballPrefab, handTransform.position, handTransform.rotation);
        activeBalls.Enqueue(newBall);
    }
}
