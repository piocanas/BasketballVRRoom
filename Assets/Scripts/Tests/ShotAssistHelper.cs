using NUnit.Framework;
using UnityEngine;

// Assist logic helper
public static class ShotAssistHelper
{
    public static Vector3 ApplyAssist(
        Vector3 velocity,
        Vector3 ballPosition,
        Vector3 hoopCenter,
        float assistStrength,
        float deltaTime)
    {
        Vector3 directionToHoop = (hoopCenter - ballPosition).normalized;

        Vector3 newVelocity = velocity;
        newVelocity.x = Mathf.Lerp(newVelocity.x, directionToHoop.x * velocity.magnitude, assistStrength * deltaTime);
        newVelocity.z = Mathf.Lerp(newVelocity.z, directionToHoop.z * velocity.magnitude, assistStrength * deltaTime);

        return newVelocity;
    }
}

public class ShotAssistHelperTests
{
    [Test]
    public void AssistImprovesDirection()
    {
        // Arrange
        Vector3 ballPos = new Vector3(0, 1, 0);
        Vector3 hoopCenter = new Vector3(5, 1, 5);
        Vector3 initialVelocity = new Vector3(1, 2, 1);
        float assistStrength = 0.5f;
        float dt = 0.02f; // Typical fixedDeltaTime

        // Act
        Vector3 assistedVelocity = ShotAssistHelper.ApplyAssist(
            initialVelocity, ballPos, hoopCenter, assistStrength, dt
        );

        // Assert: assist should reduce the angle to the target
        Vector3 targetDirection = (hoopCenter - ballPos).normalized;
        float angleBefore = Vector3.Angle(initialVelocity, targetDirection);
        float angleAfter = Vector3.Angle(assistedVelocity, targetDirection);

        Assert.Less(angleAfter, angleBefore);
    }
}
