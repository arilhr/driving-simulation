using PathCreation;
using PathCreation.Examples;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DrivingSimulation
{

    public class AutomaticGenerateRoad : Singleton<AutomaticGenerateRoad>
    {
        [Header("Props")]
        public bool generateOnAwake = false;

        [Header("Road Input")]
        public float roadLength = 1000f;
        public float roadWidth = 10f;

        [Header("Turn Input")]
        public int turns = 2;
        public float minTurnDistance = 0f;
        public float maxTurnDistance = 200f;
        public float minTurnLength = 100f;
        public float maxTurnLength = 150f;
        [Range(30f, 90f)]
        public float minTurnDegree = 45f;
        [Range(30f, 90f)]
        public float maxTurnDegree = 90f;

        [Header("Intersection Input")]
        public GameObject IntersectionPrefab = null;
        public int intersections = 0;
        public float minDistanceIntersection = 100f;

        [Header("Start/End Point")]
        public Vector3 startPoint;
        public Vector3 endPoint;

        [Header("Player")]
        public Player playerPrefabs;
        public Player playerSpawned;

        [Serializable]
        struct PathData
        {
            public PathCreator pathCreator;
            public RoadMeshCreator roadMeshCreator;
        }

        [Header("Debug")]
        public bool debug = false;
        [SerializeField]
        private List<PathData> pathDatas = new();
        [SerializeField]
        private List<Intersection> intersectionDatas = new();

        protected override void Awake()
        {
            base.Awake();

            if (generateOnAwake)
                GenerateRoad();
        }

        public void ResetData()
        {
            DestroyAllObject();

            pathDatas.Clear();
            intersectionDatas.Clear();
        }

        public void GenerateRoad()
        {
            ResetData();

            // generate path if there is intersection
            if (intersections > 0)
            {
                int roadIndex = 0;
                int turnsRemaining = turns;
                int lastIntersectDirection = -1;
                float latestIntersectionPoint = 0f;
                float maxDistanceIntersection = (roadLength - (minDistanceIntersection * 2)) / intersections;

                if (debug)
                    Debug.Log($"<color=green><b>[MAX INTERSECTION DISTANCE]</b></color> {maxDistanceIntersection}");

                for (int i = 0; i < intersections; i++)
                {
                    float intersectionPoint = Random.Range(latestIntersectionPoint + minDistanceIntersection, latestIntersectionPoint + maxDistanceIntersection);

                    if (debug)
                        Debug.Log($"<color=green><b>[INTERSECTION POINT]</b></color> {intersectionPoint}");

                    // GENERATE OR SETUP PATH BEFORE
                    if (i == 0)
                    {
                        PathCreator firstPath = GeneratePath($"Path {roadIndex}", Vector3.zero);

                        float length = intersectionPoint - latestIntersectionPoint;
                        int turn = Random.Range(0, turnsRemaining);

                        SetupPath(
                            firstPath, 
                            length, 
                            turn, 
                            GetRandomTurnDistance(turn, minTurnDistance, maxTurnDistance), 
                            GetRandomTurnLength(turn, minTurnLength, maxTurnLength),
                            true
                        );

                        if (debug)
                            Debug.Log($"<color=green><b>[GENERATE FIRST INTERSECTION PATH]</b></color> | L: {length} | T: {turn}");

                        turnsRemaining -= turn;

                        // add path to list
                        pathDatas.Add(new PathData
                        {
                            pathCreator = firstPath,
                            roadMeshCreator = firstPath.GetComponent<RoadMeshCreator>()
                        });
                    }
                    else
                    {
                        PathCreator pathBefore = pathDatas[^1].pathCreator;

                        float length = intersectionPoint - latestIntersectionPoint;
                        int turn = Random.Range(0, turnsRemaining);
                        float randomTurnDistance = GetRandomTurnDistance(turn, minTurnDistance, maxTurnDistance);
                        float randomTurnLength = GetRandomTurnLength(turn, minTurnLength, maxTurnLength);

                        SetupPath(pathBefore, length, turn, randomTurnDistance, randomTurnLength);

                        if (debug)
                            Debug.Log($"<color=green><b>[SETUP PATH BEFORE {i}]</b></color> | L: {length} | T: {turn}");

                        turnsRemaining -= turn;
                    }

                    // GENERATE INTERSECTION
                    PathCreator lastPathCreator = pathDatas[^1].pathCreator;
                    BezierPath lastBezierPath = lastPathCreator.bezierPath;
                    Vector3 lastPathPos = Quaternion.AngleAxis(lastPathCreator.transform.localEulerAngles.y, Vector3.up) * lastBezierPath.GetAnchorPoint(lastBezierPath.NumAnchorPoints - 1);
                    Vector3 intersectionPos = lastPathCreator.transform.position + lastPathPos;
                    GameObject intersectionObj = Instantiate(
                        IntersectionPrefab,
                        intersectionPos, 
                        Quaternion.identity
                    );
                    intersectionObj.name = $"Intersection {i}";
                    intersectionObj.transform.parent = transform;
                    intersectionObj.transform.localScale = new Vector3(roadWidth, roadWidth, roadWidth);

                    if (i == 0)
                    {
                        Quaternion iRot = Quaternion.FromToRotation(intersectionObj.transform.forward, lastBezierPath.LatestForward);
                        intersectionObj.transform.rotation *= iRot;
                    }
                    else
                    {
                        Vector3 firstRotatedLastPath = Quaternion.Euler(0f, lastPathCreator.transform.localEulerAngles.y, 0f) * lastBezierPath.GetAnchorPoint(lastBezierPath.NumAnchorPoints - 1);
                        Vector3 secondRotatedLastPath = Quaternion.Euler(0f, lastPathCreator.transform.localEulerAngles.y, 0f) * lastBezierPath.GetAnchorPoint(lastBezierPath.NumAnchorPoints - 2);
                        Vector3 targetForward = (firstRotatedLastPath - secondRotatedLastPath).normalized;
                        Quaternion rotation = Quaternion.FromToRotation(intersectionObj.transform.forward, targetForward);

                        intersectionObj.transform.rotation = rotation;
                        //intersectionObj.transform.eulerAngles = new Vector3(0f, intersectionObj.transform.eulerAngles.y, 0f);
                    }

                    if (intersectionObj.TryGetComponent(out Intersection intersection))
                        intersectionObj.AddComponent<Intersection>();

                    intersection.roadWidth = roadWidth;
                    Intersection.StopType intersectionType = Utils.GetRandomEnumValue<Intersection.StopType>();
                    intersection.Initialize(true, true, true, intersectionType);

                    if (debug)
                        Debug.Log($"<color=yellow><b>[GENERATE INTERSECTION {i}]</b></color> | P: {intersectionPos}");

                    // GENERATE PATH AFTER INTERSECTION
                    // next path 0: right, 1: forward, 2: left
                    PathCreator nextPathCreator = null;
                    int nextPathDirection = Random.Range(0, 3);
                    while (nextPathDirection == lastIntersectDirection) nextPathDirection = Random.Range(0, 3);
                    if (nextPathDirection == 0) nextPathCreator = intersection.right;
                    if (nextPathDirection == 1) nextPathCreator = intersection.forward;
                    if (nextPathDirection == 2) nextPathCreator = intersection.left;
                    lastIntersectDirection = nextPathDirection;

                    // SETUP LAST PATH
                    if (i == intersections - 1)
                    {
                        float length = roadLength - latestIntersectionPoint > 0 ? roadLength - latestIntersectionPoint : 0;
                        float randomTurnDistance = GetRandomTurnDistance(turnsRemaining, minTurnDistance, maxTurnDistance);
                        float randomTurnLength = GetRandomTurnLength(turnsRemaining, minTurnLength, maxTurnLength);

                        SetupPath(nextPathCreator, length, turnsRemaining, randomTurnDistance, randomTurnLength);
                    }

                    pathDatas.Add(new PathData
                    {
                        pathCreator = nextPathCreator,
                        roadMeshCreator = nextPathCreator.GetComponent<RoadMeshCreator>()
                    });
                    intersectionDatas.Add(intersection);
                    latestIntersectionPoint = intersectionPoint;
                }
            }

            if (intersections <= 0)
            {
                PathCreator path = GeneratePath($"Path 0", Vector3.zero);

                SetupPath(
                    path,
                    roadLength,
                    turns,
                    GetRandomTurnDistance(turns, minTurnDistance, maxTurnDistance),
                    GetRandomTurnLength(turns, minTurnLength, maxTurnLength)
                );

                if (debug)
                    Debug.Log($"<color=yellow><b>[GENERATE PATH]</b></color> | L: {roadLength} | T: {turns}");

                // add path to list
                pathDatas.Add(new PathData
                {
                    pathCreator = path,
                    roadMeshCreator = path.GetComponent<RoadMeshCreator>()
                });
            }

            SpawnPlayer();
        }

        private PathCreator GeneratePath(string name, Vector3 position)
        {
            // Initialize new path creator game object
            GameObject pathObj = new GameObject(name);
            pathObj.transform.position = position;
            pathObj.transform.parent = transform;
            PathCreator pathCreator = pathObj.AddComponent<PathCreator>();
            RoadMeshCreator roadMeshCreator = pathObj.AddComponent<RoadMeshCreator>();
            roadMeshCreator.roadWidth = roadWidth;
            roadMeshCreator.flattenSurface = true;
            roadMeshCreator.isRightAreaColliderActive = true;
            roadMeshCreator.pathCreator = pathCreator;

            return pathCreator;
        }

        private void SetupPath(PathCreator pathCreator, float length, int turns, float turnDistance, float turnLength, bool isStart = false)
        {
            // Initialize Path Creator
            pathCreator.InitializeEditorData(false);
            pathCreator.EditorData.ResetBezierPath(pathCreator.transform.position);

            BezierPath bp = pathCreator.bezierPath;
            List<Vector3> anchors = new();

            // set path creator bezier control mode to automatic
            bp.ControlPointMode = BezierPath.ControlMode.Automatic;
            bp.SetPoint(0, Vector3.zero);
            bp.SetPoint(bp.NumPoints - 1, isStart ? Vector3.forward * 50f : Vector3.forward);

            // get start point
            if (isStart)
            {
                startPoint = (Vector3.forward * 50f) + (Vector3.left * roadWidth / 2f);
            }

            // get init anchor points
            for (int i = 0; i < bp.NumPoints; i += 3)
            {
                anchors.Add(bp.GetPoint(i));
            }

            float turnDistanceOffset = turnDistance - (turns * minTurnDistance);
            float turnLengthOffset = turnLength - (turns * minTurnLength);
            float totalLength = 0f;

            // get forwards from last two points
            Vector3 lastForward = anchors.Count > 1 ? (anchors[^1] - anchors[^2]).normalized : Vector3.forward;

            // render turns
            for (int i = 0; i < turns; i++)
            {
                // get random distances
                float randDis = i == turns - 1? minTurnDistance + turnDistanceOffset : Random.Range(minTurnDistance, minTurnDistance + turnDistanceOffset);
                turnDistanceOffset -= (randDis - minTurnDistance);

                if (randDis > 50f)
                {
                    Vector3 newAnchor = anchors[^1] + (lastForward * randDis);
                    bp.AddSegmentToEnd(newAnchor);
                    totalLength += randDis;
                    anchors.Add(newAnchor);
                }

                // add starting anchor for turning
                Vector3 startingTurningPointAnchor = anchors[^1] + lastForward * 50f;
                bp.AddSegmentToEnd(startingTurningPointAnchor);
                anchors.Add(startingTurningPointAnchor);

                // get random lengths
                float randLength = i == turns - 1 ? minTurnLength + turnLengthOffset : Random.Range(minTurnLength, minTurnLength + turnLengthOffset);
                turnLengthOffset -= (randLength - minTurnLength);

                float turnDegree = Random.Range(minTurnDegree, maxTurnDegree);
                float totalTurnDegree = Random.Range(0, 2) == 0 ? -turnDegree : turnDegree;
                int totalTurnSegments = 2;
                for (int j = 0; j < totalTurnSegments; j++)
                {
                    lastForward = (Quaternion.AngleAxis(totalTurnDegree / 2f, Vector3.up) * lastForward).normalized;

                    Vector3 newAnchor = anchors[^1] + (lastForward * randLength / totalTurnSegments);
                    bp.AddSegmentToEnd(newAnchor);
                    anchors.Add(newAnchor);

                    if (j == totalTurnSegments - 1)
                    {
                        // add end turning point anchor
                        Vector3 endTurningPoint = anchors[^1] + lastForward * 50f;
                        bp.AddSegmentToEnd(endTurningPoint);
                        anchors.Add(endTurningPoint);
                    }
                }
            }

            float remainingLength = length - totalLength;
            if (remainingLength > 0)
            {
                Vector3 newAnchor = anchors[^1] + (lastForward * remainingLength);
                bp.AddSegmentToEnd(newAnchor);
                anchors.Add(newAnchor);
            }

            // Update road mesh creator
            RoadMeshCreator roadMeshCreator = pathCreator.GetComponent<RoadMeshCreator>();
            roadMeshCreator.TriggerUpdate();
        }

        private float GetRandomTurnDistance(int turns, float min, float max)
        {
            float minTurnDistanceAvg = min * turns;
            float maxTurnDistanceAvg = max * turns;
        
            return Random.Range(minTurnDistanceAvg, maxTurnDistanceAvg);
        }

        private float GetRandomTurnLength(int turns, float min, float max)
        {
            float minTurnAvg = min * turns;
            float maxTurnAvg = max * turns;

            return Random.Range(minTurnAvg, maxTurnAvg);
        }

        private void SpawnPlayer()
        {
            if (playerSpawned == null)
            {
                playerSpawned = Instantiate(playerPrefabs, startPoint, Quaternion.identity);
            }

            playerSpawned.transform.SetPositionAndRotation(startPoint, Quaternion.identity);
        }

        private void DestroyAllObject()
        {
            foreach (Intersection intersection in intersectionDatas)
            {
                if (intersection) intersection.DestroyObject();
            }

            foreach (PathData data in pathDatas)
            {
                if (data.roadMeshCreator) DestroyImmediate(data.roadMeshCreator.MeshHolder);
                if (data.pathCreator) DestroyImmediate(data.pathCreator.gameObject);
            }
        }

    #if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPoint, 1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(endPoint, 1f);
        }
    #endif
    }
}
