using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class UI_Manager : MonoBehaviour
    {

        [Header("UI References")]
        [SerializeField]
        private GameObject _gamePanel;
        [SerializeField]
        private GameObject _endPanel;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _gameSuccessCallback = null;

        private void Awake()
        {
            _gameSuccessCallback.AddListener(GameSuccess);
        }

        private void OnDestroy()
        {
            _gameSuccessCallback.RemoveListener(GameSuccess);
        }

        private void GameSuccess()
        {
            _gamePanel.SetActive(false);
            _endPanel.SetActive(true);
        }
    }
}
