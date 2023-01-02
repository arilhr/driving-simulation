using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class ChangeLaneTrigger : MonoBehaviour
    {
        [SerializeField]
        private string position = string.Empty;

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(ConstantVariable.PLAYER_LAYER_NAME)) return;

            Vector3 forwardDir = transform.forward;

            Vector3 directionToOther = other.transform.root.position - transform.position;

            float angle = Vector3.Angle(forwardDir, directionToOther);

            bool _playerComeFromFront = angle >= 120f;

            CurrentLaneManager playerLaneManager = other.transform.root.GetComponent<CurrentLaneManager>();

            if (playerLaneManager == null) return;

            if (_playerComeFromFront)
                playerLaneManager.ChangeFirstLane(position);
            else
                playerLaneManager.ChangeSecondLane(position);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
#endif
    }
}
