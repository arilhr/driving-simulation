using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DrivingSimulation
{
    public class LongRoadGetter : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private GameEventObject _onSetFinishLine = null;

        private Transform _finishLane = null;
        private bool _isDoneCalculated = false;


        private void Awake()
        {
            _onSetFinishLine.AddListener(OnSetFinishLine);
        }

        private void Update()
        {
            CalculateDistanceToFinish();   
        }

        private void OnDestroy()
        {
            _onSetFinishLine.RemoveListener(OnSetFinishLine);
        }

        void CalculateDistanceToFinish()
        {
            if (_isDoneCalculated) return;

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, _finishLane.position, NavMesh.AllAreas, path);

            if (path.corners.Length <= 0)
            {
                return;
            }

            float distance = 0;
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                }
            }

            if (InGamePersonaDatasetManager.Instance != null)
            {
                InGamePersonaDatasetManager.Instance.SetLongRoad(distance);
            }

            _isDoneCalculated = true;
        }

        private void OnSetFinishLine(object finishObj)
        {
            GameObject finishGameObject = (GameObject)finishObj;
            _finishLane = finishGameObject.transform;
        }

    }
}
