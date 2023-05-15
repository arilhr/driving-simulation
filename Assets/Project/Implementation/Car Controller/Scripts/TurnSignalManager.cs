using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public enum TurnSignType
    {
        None, Left, Right, Hazard
    }

    public class TurnSignalManager : MonoBehaviour
    {
        [Header("Turn Sign Game Object")]
        [SerializeField] private GameObject _turnLeftLight = null;
        [SerializeField] private GameObject _turnRightLight = null;

        [Header("Events")]
        [SerializeField] private GameEventNoParam _toogleLeftSignalCallback = null;
        [SerializeField] private GameEventNoParam _toogleRightSignalCallback = null;
        [SerializeField] private GameEventNoParam _turnOffSignalCallback = null;

        private TurnSignType _currentTurnSignal = TurnSignType.None;

        public TurnSignType CurrentTurnSignal
        {
            get { return _currentTurnSignal; }
            set
            {
                _currentTurnSignal = value;
                if (_currentTurnSignal == TurnSignType.None)
                {
                    _turnLeftLight.SetActive(false);
                    _turnRightLight.SetActive(false);
                }

                if (_currentTurnSignal == TurnSignType.Left)
                {
                    _turnLeftLight.SetActive(true);
                    _turnRightLight.SetActive(false);
                }

                if (_currentTurnSignal == TurnSignType.Right)
                {
                    _turnLeftLight.SetActive(false);
                    _turnRightLight.SetActive(true);
                }

                if (_currentTurnSignal == TurnSignType.Hazard)
                {
                    _turnLeftLight.SetActive(true);
                    _turnRightLight.SetActive(true);
                }
            }
        }

        #region Mono

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _toogleLeftSignalCallback.AddListener(ToogleTurnLeftSignal);
            _toogleRightSignalCallback.AddListener(ToogleTurnRightSignal);
            _turnOffSignalCallback.AddListener(TurnOffSignal);
        }

        private void Dispose()
        {
            _toogleLeftSignalCallback.RemoveListener(ToogleTurnLeftSignal);
            _toogleRightSignalCallback.RemoveListener(ToogleTurnRightSignal);
            _turnOffSignalCallback.RemoveListener(TurnOffSignal);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region Methods

        private void ToogleTurnLeftSignal()
        {
            if (_currentTurnSignal == TurnSignType.Left)
            {
                CurrentTurnSignal = TurnSignType.None;
                return;
            }

            CurrentTurnSignal = TurnSignType.Left;
        }

        private void ToogleTurnRightSignal()
        {
            if (_currentTurnSignal == TurnSignType.Right)
            {
                CurrentTurnSignal = TurnSignType.None;
                return;
            }

            CurrentTurnSignal = TurnSignType.Right;
        }

        private void TurnOffSignal()
        {
            CurrentTurnSignal = TurnSignType.None;
        }

        #endregion
    }
}
