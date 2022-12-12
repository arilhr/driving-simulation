using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
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
        private GameObject _gameLostPanel;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _gameWinCallback = null;
        [SerializeField]
        private GameEventNoParam _gameLostCallback = null;
        [SerializeField]
        private GameEventNoParam _nextLevelCallback = null;

        private void Awake()
        {
            _gameWinCallback.AddListener(GameWin);
            _gameLostCallback.AddListener(GameLost);
        }

        private void OnDestroy()
        {
            _gameWinCallback.RemoveListener(GameWin);
            _gameLostCallback.RemoveListener(GameLost);
        }

        private void GameWin()
        {
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
    }
}
