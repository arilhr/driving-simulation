using Sirenix.OdinInspector;
using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class MinimapCameraFollow : MonoBehaviour
    {
        [BoxGroup("Properties")]
        [SerializeField]
        private Transform _objectToFollow = null;
        [BoxGroup("Properties")]
        [SerializeField]
        private Vector3 _offset = Vector3.zero;

        [BoxGroup("Events")]
        public GameEventNoParam InitializeGameCallback = null;

        #region Mono

        private void Awake()
        {
            InitializeGameCallback?.AddListener(OnInitializeGame);
        }

        private void Update()
        {
            if (_objectToFollow == null) 
                return;

            UpdateCameraPosition();
            UpdateCameraRotation();
        }

        private void OnDestroy()
        {
            InitializeGameCallback?.RemoveListener(OnInitializeGame);
        }

        #endregion

        #region Method

        void OnInitializeGame()
        {
            // Find player
            if (Player.Instance == null)
                return;

            _objectToFollow = Player.Instance.objToFollow.transform;
        }

        void UpdateCameraPosition()
        {
            transform.position = _objectToFollow.position + _offset;
        }

        void UpdateCameraRotation()
        {
            Vector3 euler = new Vector3(transform.rotation.eulerAngles.x, _objectToFollow.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(euler);
        }

        #endregion

    #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_objectToFollow == null) return;

            UpdateCameraPosition();
        }
    #endif
    }
}
