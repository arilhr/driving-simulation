using Sirenix.OdinInspector;
using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    enum SpeedLimitType
    {
        Maximum,
        Minimum
    }

    public class SpeedLimitTrigger : MonoBehaviour
    {
        #region Variables


        [BoxGroup("Properties")]
        [SerializeField]
        private bool _isReverse = false;

        [BoxGroup("Properties")]
        [SerializeField]
        private SpeedLimitType _type = SpeedLimitType.Maximum;

        [BoxGroup("Properties")]
        [SerializeField]
        private float _speedLimit = 0f;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventBool _setActiveMaxLimit = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventBool _setActiveMinLimit = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventFloat _changeMaxLimit = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventFloat _changeMinLimit = null;

        #endregion

        #region Mono

        private void OnTriggerEnter(Collider other)
        {
            Vector3 toTarget = (other.transform.position - transform.position).normalized;

            SetActiveLimit(!(Vector3.Dot(toTarget, transform.position) > 0) && !_isReverse);
        }

        private void OnTriggerExit(Collider other)
        {
            Vector3 toTarget = (other.transform.position - transform.position).normalized;

            SetActiveLimit(!(Vector3.Dot(toTarget, transform.position) > 0) && !_isReverse);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
#endif
        }

        #endregion

        #region Methods

        private void SetActiveLimit(bool active)
        {
            switch (_type)
            {
                case SpeedLimitType.Maximum:
                    Debug.Log($"Max limit on {active} | {_speedLimit}");
                    _changeMaxLimit.Invoke(_speedLimit);
                    _setActiveMaxLimit.Invoke(active);
                    break;
                case SpeedLimitType.Minimum:
                    Debug.Log($"Min limit on {active} | {_speedLimit}");
                    _changeMinLimit.Invoke(_speedLimit);
                    _setActiveMinLimit.Invoke(active);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
