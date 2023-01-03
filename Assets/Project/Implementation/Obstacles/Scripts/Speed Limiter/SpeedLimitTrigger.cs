using Sirenix.OdinInspector;
using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [BoxGroup("References")]
        [SerializeField]
        private BoxCollider _triggerCollider;

        [BoxGroup("UI")]
        [SerializeField]
        private TMP_Text _speedLimitText;

        [BoxGroup("UI")]
        [SerializeField]
        private GameObject _maxUI;

        [BoxGroup("UI")]
        [SerializeField]
        private GameObject _minUI;

        [BoxGroup("UI")]
        [SerializeField]
        private GameObject _slashObj;

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

        private void Start()
        {
            UI_Initialize();

            Transform roadObj = transform.parent.parent.parent.parent;
            GSDRoad road = roadObj.GetComponent<GSDRoad>();

            if (road == null) return;

            _triggerCollider.size = new Vector3(road.opt_LaneWidth * 2, 10f, 1f);
            _triggerCollider.center = new Vector3(road.opt_LaneWidth, 5f, 0);
        }

        private void UI_Initialize()
        {
            _speedLimitText.text = _speedLimit.ToString();

            if (_type == SpeedLimitType.Maximum)
            {
                _maxUI.SetActive(true);
                _speedLimitText.color = Color.black;
            }

            if (_type == SpeedLimitType.Minimum)
            {
                _minUI.SetActive(true);
                _speedLimitText.color = Color.white;
            }

            _slashObj.SetActive(_isReverse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(ConstantVariable.PLAYER_LAYER_NAME)) return;

            Vector3 forwardDir = transform.forward;

            Vector3 directionToOther = other.transform.root.position - transform.position;

            float angle = Vector3.Angle(forwardDir, directionToOther);

            if (_isReverse)
                SetActiveLimit(angle < 90f);
            else
                SetActiveLimit(angle >= 90f);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
#endif

        #endregion

        #region Methods

        private void SetActiveLimit(bool active)
        {
            switch (_type)
            {
                case SpeedLimitType.Maximum:
                    _changeMaxLimit.Invoke(_speedLimit);
                    _setActiveMaxLimit.Invoke(active);
                    break;
                case SpeedLimitType.Minimum:
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
