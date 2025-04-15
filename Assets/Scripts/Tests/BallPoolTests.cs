using NUnit.Framework;
using UnityEngine;

public class SpawnBallTests
{
    private GameObject spawnObj;
    private SpawnBall spawner;
    private Transform dummyHand;

    [SetUp]
    public void Setup()
    {
        spawnObj = new GameObject("Spawner");
        spawner = spawnObj.AddComponent<SpawnBall>();

        spawner.ballPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere); // dummy ball
        dummyHand = new GameObject("Hand").transform;
        dummyHand.position = Vector3.zero;
        dummyHand.rotation = Quaternion.identity;

        spawner.handTransform = dummyHand;
        spawner.maxBalls = 5;
    }

    [Test]
    public void SpawnBall_LimitsActiveBalls()
    {
        // Spawn 7 balls — only 5 should remain
        for (int i = 0; i < 7; i++)
        {
            spawner.Spawn();
        }

        Assert.AreEqual(5, spawner.activeBalls.Count);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(spawnObj);
        Object.DestroyImmediate(dummyHand.gameObject);
    }
}

