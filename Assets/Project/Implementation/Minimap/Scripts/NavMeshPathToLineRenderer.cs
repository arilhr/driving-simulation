using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DrivingSimulation
{
    public class NavMeshPathToLineRenderer : MonoBehaviour
    {
        [SerializeField]
        private Transform _startPosition;

        [SerializeField]
        private Transform _endPosition;

        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private float _lineWidth = 1f;

        private NavMeshPath _path = null;

        private void Awake()
        {
            _path = new();

            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
        }

        private void Update()
        {
            DisplayLineRenderer();
        }

        private void DisplayLineRenderer()
        {
            NavMesh.CalculatePath(_startPosition.position, _endPosition.position, NavMesh.AllAreas, _path);

            // Create a list to hold the smooth points of the path
            List<Vector3> smoothPoints = new List<Vector3>();

            // Add the start point of the path to the list of smooth points
            smoothPoints.Add(_path.corners[0]);

            // Iterate through the points in the path and add smooth points between each pair of points
            for (int i = 0; i < _path.corners.Length - 1; i++)
            {
                Vector3 p0 = _path.corners[i];
                Vector3 p1 = (_path.corners[i] + _path.corners[i + 1]) / 2;
                Vector3 p2 = _path.corners[i + 1];

                // Add a smooth point at the midpoint between p0 and p1
                smoothPoints.Add(GetBezierPoint(p0, p1, p2, 0.5f));

                // Add the endpoint of the segment
                smoothPoints.Add(p2);
            }

            // Set the number of points in the LineRenderer to the number of points in the NavMeshPath
            _lineRenderer.positionCount = smoothPoints.Count;

            // Iterate through the points in the NavMeshPath and set the corresponding point in the LineRenderer
            for (int i = 0; i < smoothPoints.Count; i++)
            {
                Vector3 finalPos = smoothPoints[i] + new Vector3(0, 1f, 0);
                _lineRenderer.SetPosition(i, finalPos);
            }
        }

        Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            // Calculate the point on the curve using the Bezier formula
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;
            return p;
        }
    }
}
