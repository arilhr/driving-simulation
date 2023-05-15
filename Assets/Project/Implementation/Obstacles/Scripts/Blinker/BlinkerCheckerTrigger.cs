using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class BlinkerCheckerTrigger : MonoBehaviour
    {
        private const string PLAYER_LAYER_NAME = "Player";

        private BlinkerChecker blickCheckerManager = null;

        private void Awake()
        {
            if (!transform.parent.TryGetComponent(out blickCheckerManager))
            {
                blickCheckerManager = transform.parent.gameObject.AddComponent<BlinkerChecker>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(PLAYER_LAYER_NAME)) return;

            Vector3 forwardDir = 1f * transform.forward;

            Vector3 directionToOther = other.transform.root.position - transform.position;

            float angle = Vector3.Angle(forwardDir, directionToOther);

            if (angle < 90f)
            {
                blickCheckerManager.PosBefore = transform;
            }
            else
            {
                blickCheckerManager.CheckBlinker(transform);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.5f);
        }
#endif
    }
}
