using Cinemachine;
using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class CameraManager : Singleton<CameraManager>
    {
        [Header("References")]
        public CinemachineVirtualCamera vcam;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _initializeGame = null;

        protected override void Awake()
        {
            base.Awake();

            _initializeGame.AddListener(Initialize);
        }

        private void OnDestroy()
        {
            _initializeGame.RemoveListener(Initialize);
        }

        void Initialize()
        {
            if (Player.Instance == null) return;

            vcam.LookAt = Player.Instance.objToFollow.transform;
            vcam.Follow = Player.Instance.objToFollow.transform;
        }
    }
}
