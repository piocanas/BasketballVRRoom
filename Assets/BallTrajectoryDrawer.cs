using UnityEngine;
using System.Collections.Generic;

public class BallTrajectoryDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int resolution = 30;
    public float timeStep = 0.1f;
    public float maxTime = 2f;

    private bool showTrajectory = false;
    private Vector3 initialPosition;
    private Vector3 initialVelocity;

    public void ShowTrajectory(Vector3 startPos, Vector3 velocity)
    {
        initialPosition = startPos;
        initialVelocity = velocity;
        showTrajectory = true;

        RenderTrajectory();
    }

    private void RenderTrajectory()
    {
        List<Vector3> points = new List<Vector3>();

        Vector3 currentPosition = initialPosition;
        Vector3 currentVelocity = initialVelocity;

        for (int i = 0; i < resolution; i++)
        {
            points.Add(currentPosition);
            currentVelocity += Physics.gravity * timeStep;
            currentPosition += currentVelocity * timeStep;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
