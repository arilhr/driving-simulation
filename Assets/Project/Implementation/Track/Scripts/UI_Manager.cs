using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrivingSimulation
{
    public class UI_Manager : MonoBehaviour
    {

        [Header("UI References")]
        [SerializeField]
        private GameObject _gamePanel;
        [SerializeField]
        private GameObject _gameWinPanel;
        [SerializeField]
        private TMP_Text _pointText;
        [SerializeField]
        private GameObject _gameLostPanel;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _gameWinCallback = null;
        [SerializeField]
        private GameEventNoParam _gameLostCallback = null;
        [SerializeField]
        private GameEventNoParam _nextLevelCallback = null;
        [SerializeField]
        private GameEventInt _onPointChanged = null;

        private int _currentPoint = 0;


        private void Awake()
        {
            _gameWinCallback.AddListener(GameWin);
            _gameLostCallback.AddListener(GameLost);
            _onPointChanged.AddListener(OnPointChanged);
        }

        private void OnDestroy()
        {
            _gameWinCallback.RemoveListener(GameWin);
            _gameLostCallback.RemoveListener(GameLost);
            _onPointChanged.RemoveListener(OnPointChanged);
        }

        private void GameWin()
        {
            _pointText.text = "Point: " + _currentPoint.ToString();
            _gamePanel.SetActive(false);
            _gameWinPanel.SetActive(true);
        }

        private void GameLost()
        {
            _gamePanel.SetActive(false);
            _gameLostPanel.SetActive(true);
        }

        public void NextLevel()
        {
            _nextLevelCallback.Invoke();
        }

        public void RetryLevel()
        {
            SceneManager.LoadScene(gameObject.scene.name);
        }

        public void BackMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        private void OnPointChanged(int point)
        {
            _currentPoint = point;
        }
    }
}
