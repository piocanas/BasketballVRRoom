using NUnit.Framework;
using UnityEngine;

// Mock ScoreManager for testing
public class ScoreManager
{
    private int score = 0;

    public bool OnBallEnterHoop(GameObject ball)
    {
        // Simulate scoring logic
        score += 1;
        return true;
    }

    public int GetScore()
    {
        return score;
    }
}

public class ScoreManagerTests
{
    [Test]
    public void ScoreIncrementsOnHoopTrigger()
    {
        // Arrange
        ScoreManager scoreManager = new ScoreManager();
        GameObject mockBall = new GameObject("MockBall");

        // Act
        bool triggered = scoreManager.OnBallEnterHoop(mockBall);

        // Assert
        Assert.IsTrue(triggered);
        Assert.AreEqual(1, scoreManager.GetScore());
    }
}
