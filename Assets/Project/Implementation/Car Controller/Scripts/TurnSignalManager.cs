using Sirenix.OdinInspector;
using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class TurnSignalManager : MonoBehaviour
    {
        [BoxGroup("References")]
        [SerializeField]
        private GameObject _turnSignalLight = null;
        
        [BoxGroup("Events")]
        [SerializeField]
        private GameEventNoParam _toogleTurnSignalCallback = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventBool _onTurnSignalChanged = null;

        [BoxGroup("Debug", Order = 100)]
        private bool _isActive = false;

        #region Properties

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                _onTurnSignalChanged.Invoke(_isActive);
            }
        }

        #endregion

        #region Mono

        private void Awake()
        {
            Initialize();

            IsActive = false;
        }

        private void Initialize()
        {
            _toogleTurnSignalCallback.AddListener(Toogle);
            _onTurnSignalChanged.AddListener(OnTurnSignalChanged);
        }

        private void Dispose()
        {
            _toogleTurnSignalCallback.RemoveListener(Toogle);
            _onTurnSignalChanged.RemoveListener(OnTurnSignalChanged);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region Methods

        private void Toogle()
        {
            IsActive = !IsActive;
        }

        private void OnTurnSignalChanged(bool isActive)
        {
            _turnSignalLight.SetActive(isActive);
        }

        #endregion
    }
}
