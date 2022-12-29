using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class WrongAreaTrigger : MonoBehaviour
    {
        private const string WRONG_LANE_KEY = "Wrong Lane!";

        private bool _playerInArea = false;

        private const float _exitTime = 1f;

        private float _delayViolated = 3f;

        private float _lastExitTime = 0;
        private float _currentViolatedTime = 0f;

        private void Awake()
        {
            transform.TryGetComponent(out Rigidbody rb);

            if (rb == null)
            {
                Rigidbody newRb = gameObject.AddComponent<Rigidbody>();
                newRb.isKinematic = true;   
            }
        }

        private void Update()
        {
            CheckPlayerExited();
            CheckPlayerViolated();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(ConstantVariable.PLAYER_LAYER_NAME)) return;

            _playerInArea = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(ConstantVariable.PLAYER_LAYER_NAME)) return;

            _lastExitTime = Time.time;
        }

        private void CheckPlayerViolated()
        {
            if (!_playerInArea) return;

            if (_currentViolatedTime <= 0)
            {
                _currentViolatedTime = _delayViolated;
                Violated();
            }

            _currentViolatedTime -= Time.deltaTime;
        }

        private void CheckPlayerExited()
        {
            if (!_playerInArea) return;

            if (_lastExitTime + _exitTime <= Time.time)
            {
                _currentViolatedTime = 0;
                _playerInArea = false;
            }
        }

        private void Violated()
        {
            if (GlobalEvents.Instance == null) return;

            GlobalEvents.Instance.SetNotificationCallback.Invoke("Wrong Lane!", (int)NotificationType.Danger);
            GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

            GlobalEvents.Instance.AddPointCallback.Invoke(-10);

            if (GlobalEvents.Instance.AddMistakeCallback != null)
                GlobalEvents.Instance.AddMistakeCallback.Invoke(WRONG_LANE_KEY, 1);
        }
    }
}
