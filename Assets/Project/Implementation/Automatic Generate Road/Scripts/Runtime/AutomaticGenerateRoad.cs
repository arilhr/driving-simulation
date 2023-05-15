using PathCreation;
using PathCreation.Examples;
using SOGameEvents;
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

        [Header("Road Settings")]
        public Material railingMaterial = null;
        public float maxVertexError = 5f;

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
        public GameObject barrierObject = null;
        public GameObject finishAreaObj;

        [Header("Player")]
        public Player playerPrefabs;
        public Player playerSpawned;

        [Header("Events")]
        [SerializeField] 
        private GameEventNoParam InitializeGame = null;

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

        private float latestLeftTurnDegree = 0f;
        private float latestRightTurnDegree = 0f;

        public float LatestLeftTurnDegree
        {
            get { return latestLeftTurnDegree; }
            private set
            {
                latestLeftTurnDegree = value;

                if (latestLeftTurnDegree > 0f)
                    latestLeftTurnDegree = 0f;
            }
        }

        public float LatestRightTurnDegree
        {
            get { return latestRightTurnDegree; }
            private set
            {
                latestRightTurnDegree = value;

                if (latestRightTurnDegree < 0f)
                    latestRightTurnDegree = 0f;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (generateOnAwake)
                GenerateRoad();
        }

        public void ResetData()
        {
            LatestLeftTurnDegree = 0;
            LatestRightTurnDegree = 0;
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
                float minStartLength = 100f;
                int lastIntersectDirection = -1;
                float lengthRemaining = roadLength;

                for (int i = 0; i < intersections; i++)
                {
                    // GENERATE START PATH
                    if (i == 0)
                    {
                        PathCreator firstPath = GeneratePath($"Path {roadIndex}", Vector3.zero);

                        float startLength = minStartLength < lengthRemaining ? Random.Range(minStartLength, lengthRemaining) : minStartLength;
                        int startTurn = Random.Range(0, turnsRemaining);

                        SetupPath(
                            firstPath,
                            startLength,
                            startTurn, 
                            GetRandomTurnDistance(startTurn, minTurnDistance, maxTurnDistance), 
                            GetRandomTurnLength(startTurn, minTurnLength, maxTurnLength),
                            true
                        );

                        if (debug)
                            Debug.Log($"<color=green><b>[GENERATE FIRST INTERSECTION PATH]</b></color> | L: {startLength} | T: {startTurn}");

                        turnsRemaining -= startTurn;
                        lengthRemaining -= startLength;

                        // add start path to list
                        pathDatas.Add(new PathData
                        {
                            pathCreator = firstPath,
                            roadMeshCreator = firstPath.GetComponent<RoadMeshCreator>()
                        });
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
                    }

                    if (intersectionObj.TryGetComponent(out Intersection intersection))
                        intersectionObj.AddComponent<Intersection>();

                    intersection.roadWidth = roadWidth;
                    Intersection.StopType intersectionType = Utils.GetRandomEnumValue<Intersection.StopType>();
                    intersection.InitializeRoadData(maxVertexError, railingMaterial, true);
                    intersection.Initialize(true, true, true, intersectionType);

                    if (debug)
                        Debug.Log($"<color=yellow><b>[GENERATE INTERSECTION {i}]</b></color> | P: {intersectionPos}");

                    // GENERATE PATH AFTER INTERSECTION
                    // next path 0: right, 1: forward, 2: left
                    PathCreator nextPathCreator = null;
                    List<int> nextDirectionIndex = new List<int>{ 0, 1, 2 };
                    if (LatestRightTurnDegree > 90f)
                        nextDirectionIndex.Remove(0);

                    if (LatestLeftTurnDegree < -90f)
                        nextDirectionIndex.Remove(2);

                    int nextPathDirection = nextDirectionIndex[Random.Range(0, nextDirectionIndex.Count)];
                    while (nextPathDirection == lastIntersectDirection) 
                        nextPathDirection = nextDirectionIndex[Random.Range(0, nextDirectionIndex.Count)];
                    if (nextPathDirection == 0) nextPathCreator = intersection.right;
                    if (nextPathDirection == 1) nextPathCreator = intersection.forward;
                    if (nextPathDirection == 2) nextPathCreator = intersection.left;
                    lastIntersectDirection = nextPathDirection;

                    intersection.SetRoadEnd(nextPathDirection != 0,
                                            nextPathDirection != 1,
                                            nextPathDirection != 2,
                                            barrierObject,
                                            10f);

                    // GENERATE NEXT PATH (AFTER INTERSECTION)
                    float length = Random.Range(minDistanceIntersection, lengthRemaining > minDistanceIntersection ? lengthRemaining : minDistanceIntersection);
                    int turn = Random.Range(0, turnsRemaining);
                    float randomTurnDistance = GetRandomTurnDistance(turn, minTurnDistance, maxTurnDistance);
                    float randomTurnLength = GetRandomTurnLength(turn, minTurnLength, maxTurnLength);

                    Debug.Log(length);

                    float initialDegree = lastIntersectDirection == 0 ? 90f : lastIntersectDirection == 2 ? -90f : 0f;

                    SetupPath(nextPathCreator, length, turn, randomTurnDistance, randomTurnLength, false, initialDegree);

                    if (debug)
                        Debug.Log($"<color=green><b>[SETUP PATH BEFORE {i}]</b></color> | L: {length} | T: {turn}");

                    turnsRemaining -= turn;
                    lengthRemaining -= length;

                    // SETUP LAST PATH (AFTER INTERSECTION)
                    if (i == intersections - 1)
                    {
                        // spawn finish area
                        SpawnFinishArea(nextPathCreator);
                    }

                    pathDatas.Add(new PathData
                    {
                        pathCreator = nextPathCreator,
                        roadMeshCreator = nextPathCreator.GetComponent<RoadMeshCreator>()
                    });

                    intersectionDatas.Add(intersection);
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
                    GetRandomTurnLength(turns, minTurnLength, maxTurnLength),
                    true
                );

                if (debug)
                    Debug.Log($"<color=yellow><b>[GENERATE PATH]</b></color> | L: {roadLength} | T: {turns}");

                // add path to list
                pathDatas.Add(new PathData
                {
                    pathCreator = path,
                    roadMeshCreator = path.GetComponent<RoadMeshCreator>()
                });

                SpawnFinishArea(path);
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
            roadMeshCreator.isLeftRailingActive = true;
            roadMeshCreator.isRightRailingActive = true;
            roadMeshCreator.railingMaterial = railingMaterial;
            roadMeshCreator.pathCreator = pathCreator;

            return pathCreator;
        }

        private void SetupPath(PathCreator pathCreator, float length, int turns, float turnDistance, float turnLength, bool isStart = false, float initialDegree = 0f)
        {
            LatestLeftTurnDegree += initialDegree;
            LatestRightTurnDegree += initialDegree;

            // Initialize Path Creator
            pathCreator.InitializeEditorData(false);
            pathCreator.EditorData.ResetBezierPath(pathCreator.transform.position);
            pathCreator.EditorData.vertexPathMaxAngleError = maxVertexError;

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

                // spawn start barrier
                GameObject startBarrier = Instantiate(barrierObject, transform);
                startBarrier.name = "Start Barrier";

                if (!startBarrier.TryGetComponent(out RepeatedObject startRepeatedObj))
                    startRepeatedObj = startBarrier.AddComponent<RepeatedObject>();

                startRepeatedObj.transform.localPosition = Vector3.forward * 20f;

                startRepeatedObj.width = roadWidth * 2f;
                startRepeatedObj.Build();
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

                int totalTurnSegments = 2;
                float turnDegree = Random.Range(minTurnDegree, maxTurnDegree);
                float totalTurnDegree = Random.Range(0, 2) == 0 ? -turnDegree : turnDegree;

                if (LatestRightTurnDegree + turnDegree > 180f)
                {
                    totalTurnDegree = -turnDegree;
                }

                if (LatestLeftTurnDegree - turnDegree < -180f)
                {
                    totalTurnDegree = turnDegree;
                }

                LatestLeftTurnDegree += totalTurnDegree;
                LatestRightTurnDegree += totalTurnDegree;

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


        private void SpawnFinishArea(PathCreator path)
        {
            // spawn finish area
            Vector3 lastPoint1 = Quaternion.AngleAxis(path.transform.localEulerAngles.y, Vector3.up) * path.bezierPath.GetAnchorPoint(path.bezierPath.NumAnchorPoints - 1);
            Vector3 lastPoint2 = Quaternion.AngleAxis(path.transform.localEulerAngles.y, Vector3.up) * path.bezierPath.GetAnchorPoint(path.bezierPath.NumAnchorPoints - 2);

            Vector3 lastDirection = (lastPoint2 - lastPoint1).normalized;

            GameObject finishAreaSpawned = Instantiate(finishAreaObj, transform);
            Vector3 finishPos = path.transform.position + lastPoint1 + (lastDirection * 50f) - (Vector3.Cross(lastDirection, Vector3.up) * roadWidth / 2f);
            Quaternion finishRot = Quaternion.FromToRotation(finishAreaSpawned.transform.forward, lastDirection);

            finishAreaSpawned.transform.localPosition = finishPos;
            finishAreaSpawned.transform.rotation = finishRot;

            // spawn barrier
            Vector3 finishBarrierPos = path.transform.position + lastPoint1 + (lastDirection * 20f);

            // spawn finish barrier
            GameObject finishBarrierObj = Instantiate(barrierObject, transform);
            finishBarrierObj.name = "Finish Barrier";

            if (!finishBarrierObj.TryGetComponent(out RepeatedObject finishRepeatedObj))
                finishRepeatedObj = finishBarrierObj.AddComponent<RepeatedObject>();

            finishRepeatedObj.transform.localPosition = finishBarrierPos;
            finishRepeatedObj.transform.rotation = Quaternion.FromToRotation(finishBarrierObj.transform.forward, lastDirection);

            finishRepeatedObj.width = roadWidth * 2f;
            finishRepeatedObj.Build();
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

            InitializeGame.Invoke();
        }

        private void DestroyAllObject()
        {
            // Get all child objects of this game object
            Transform[] children = GetComponentsInChildren<Transform>();

            // Loop through each child and destroy it
            foreach (Transform child in children)
            {
                // Skip the parent object
                if (child == transform) continue;
                if (child == null) continue;

                // Destroy the child object
                DestroyImmediate(child.gameObject);
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
