using Sirenix.OdinInspector;
using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class FinishTrigger : MonoBehaviour
    {
        [BoxGroup("Settings")]
        [SerializeField]
        private LayerMask _playerLayer;
        [BoxGroup("Settings")]
        [SerializeField]
        private float _maxSpeedToWin = 5f;

        private bool isAlreadyEnded = false;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventNoParam _gameSuccessCallback = null;
        [BoxGroup("Events")]
        [SerializeField]
        private GameEventObject _onSetFinishLine = null;

        private void Start()
        {
            _onSetFinishLine.Invoke(gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (isAlreadyEnded) return;
            if (!((_playerLayer.value & (1 << other.gameObject.layer)) > 0)) return;

            if (!other.transform.root.TryGetComponent(out VehicleController playerVehicle)) return;
            if (Mathf.Floor(playerVehicle.KMh) > _maxSpeedToWin) return;

            isAlreadyEnded = true;

            _gameSuccessCallback?.Invoke();
        }
    }
}
