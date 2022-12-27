using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class StopSignTrigger : MonoBehaviour
    {
        private bool _playerComeFromFront = false;
        private bool _alreadyPassed = false;
        private bool _isSuccess = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_alreadyPassed) return;
            if (other.gameObject.layer != LayerMask.NameToLayer(ConstantVariable.PLAYER_LAYER_NAME)) return;

            Vector3 forwardDir = transform.forward;

            Vector3 directionToOther = other.transform.root.position - transform.position;

            float angle = Vector3.Angle(forwardDir, directionToOther);

            _playerComeFromFront = angle >= 90f;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_alreadyPassed) return;
            if (other.gameObject.layer != LayerMask.NameToLayer(ConstantVariable.PLAYER_LAYER_NAME)) return;
            if (!_playerComeFromFront) return;

            other.transform.root.TryGetComponent(out VehicleController controller);

            if (controller == null) return;

            if (Mathf.FloorToInt(controller.KMh) == 0)
            {
                Success();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_playerComeFromFront) return;

            if (!_isSuccess)
                Failed();

            _playerComeFromFront = false;
            _alreadyPassed = true;
        }

        private void Success()
        {
            _alreadyPassed = true;
            _isSuccess = true;

            if (GlobalEvents.Instance == null) return;

            // Notification
            GlobalEvents.Instance.SetNotificationCallback.Invoke("Stop Sign!", (int)NotificationType.Success);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            // Points
            GlobalEvents.Instance.AddPointCallback.Invoke(10);
        }

        private void Failed()
        {
            _alreadyPassed = true;

            if (GlobalEvents.Instance == null) return;

            // Notification
            GlobalEvents.Instance.SetNotificationCallback.Invoke("Stop Sign!", (int)NotificationType.Danger);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            // Points
            GlobalEvents.Instance.AddPointCallback.Invoke(-10);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
    }
#endif
}
