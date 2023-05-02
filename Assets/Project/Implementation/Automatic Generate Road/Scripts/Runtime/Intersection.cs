using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public enum StopType
    {
        None,
        Stop,
        TrafficLight
    }

    [Header("Properties")]
    public PathCreator forward = null;
    public PathCreator left = null;
    public PathCreator right = null;
    public float roadWidth = 10f;
    public float initialRoadLength = 5f;
    public StopType stopType = StopType.None;

    [Header("Traffic Light")]
    public TrafficLightManager trafficLightManager;
    public TrafficLight trafficLightB;
    public TrafficLight trafficLightF;
    public TrafficLight trafficLightR;
    public TrafficLight trafficLightL;

    [Header("Stop Sign")]
    public List<GameObject> stopSigns;

    [Header("Material")]
    public Material LRMaterial;
    public Material RFMaterial;
    public Material LFMaterial;
    public Material LRFMaterial;

    [Header("Debug")]
    public float anchorRadius = 1f;

    public void Initialize(bool right, bool forward, bool left)
    {
        if (forward)
        {
            Vector3 forwardPos = transform.position + transform.forward * (transform.localScale.z * 2);
            this.forward = GeneratePath("Forward", forwardPos, transform.forward);
        }

        if (right)
        {
            Vector3 rightPos = transform.position + transform.right * (transform.localScale.x) + transform.forward * (transform.localScale.z);
            this.right = GeneratePath("Right", rightPos, transform.right);
        }

        if (left)
        {
            Vector3 leftPos = transform.position - transform.right * (transform.localScale.x) + transform.forward * (transform.localScale.z);
            this.left = GeneratePath("Left", leftPos, -transform.right);
        }
    }

    public void Initialize(bool right, bool forward, bool left, StopType type)
    {
        Initialize(right, forward, left);

        // Initialize Stop Type
        if (type == StopType.TrafficLight)
        {
            InitializeTrafficLight(forward, right, left);
        }

        if (type == StopType.Stop)
        {
            InitializeStopSign(forward, right, left);
        }
    }

    private void InitializeTrafficLight(bool f, bool r, bool l)
    {
        List<TrafficLight> trafficLightActive = new List<TrafficLight>();

        trafficLightB.gameObject.SetActive(true);
        trafficLightActive.Add(trafficLightB);

        if (f)
        {
            trafficLightF.gameObject.SetActive(true);
            trafficLightActive.Add(trafficLightF);
        }

        if (r)
        {
            trafficLightR.gameObject.SetActive(true);
            trafficLightActive.Add(trafficLightR);
        }

        if (l)
        {
            trafficLightL.gameObject.SetActive(true);
            trafficLightActive.Add(trafficLightL);
        }

        float interval = Mathf.CeilToInt(Random.Range(5f, 10f));
        trafficLightManager.Initialize(interval, trafficLightActive);
    }

    private void InitializeStopSign(bool f, bool r, bool l)
    {
        stopSigns[0].SetActive(true);

        if (l) stopSigns[1].SetActive(true);
        if (f) stopSigns[2].SetActive(true);
        if (r) stopSigns[3].SetActive(true);
    }

    private PathCreator GeneratePath(string name, Vector3 position, Vector3 direction)
    {
        GameObject path = new($"{gameObject.name} - {name}");
        path.transform.SetPositionAndRotation(position, Quaternion.FromToRotation(path.transform.forward, direction));
        path.transform.parent = transform.parent;

        // add path creator
        PathCreator pathCreator = path.AddComponent<PathCreator>();

        // Initialize path creator
        pathCreator.InitializeEditorData(false);
        pathCreator.EditorData.ResetBezierPath(path.transform.localPosition);
        BezierPath bp = pathCreator.bezierPath;

        // set path creator bezier control mode to automatic
        bp.SetPoint(0, Vector3.zero);
        bp.SetPoint(bp.NumPoints - 1, Vector3.forward * initialRoadLength);
        bp.ControlPointMode = BezierPath.ControlMode.Automatic;

        // add road creator
        RoadMeshCreator roadMeshCreator = path.AddComponent<RoadMeshCreator>();
        roadMeshCreator.roadWidth = roadWidth;
        roadMeshCreator.pathCreator = pathCreator;
        roadMeshCreator.flattenSurface = true;
        roadMeshCreator.TriggerUpdate();

        return pathCreator;
    }

    public void DestroyObject()
    {
        if (forward)
        {
            if (forward.TryGetComponent(out RoadMeshCreator forwardRoadCreator))
            {
                DestroyImmediate(forwardRoadCreator.MeshHolder);
            }

            DestroyImmediate(forward.gameObject);
        }

        if (right)
        {
            if (right.TryGetComponent(out RoadMeshCreator rightRoadCreator))
            {
                DestroyImmediate(rightRoadCreator.MeshHolder);
            }

            DestroyImmediate(right.gameObject);
        }

        if (left)
        {
            if (left.TryGetComponent(out RoadMeshCreator leftRoadCreator))
            {
                DestroyImmediate(leftRoadCreator.MeshHolder);
            }

            DestroyImmediate(left.gameObject);
        }

        DestroyImmediate(gameObject);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position, anchorRadius);
        if (forward) Gizmos.DrawSphere(transform.position + transform.forward * (transform.localScale.z * 2), anchorRadius);
        if (right) Gizmos.DrawSphere(transform.position + transform.right * (transform.localScale.x) + transform.forward * (transform.localScale.z), anchorRadius);
        if (left) Gizmos.DrawSphere(transform.position - transform.right * (transform.localScale.x) + transform.forward * (transform.localScale.z), anchorRadius);
    }
#endif
}
