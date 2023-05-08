using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _gameWinCallback = null;
        [SerializeField]
        private GameEventNoParam _gameLostCallback = null;

        [Header("Input Events")]
        [SerializeField]
        private GameEventBool _setInputActiveCallback = null;

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

        private void GameEnd()
        {
            _setInputActiveCallback.Invoke(false);

            if (InGamePersonaDatasetManager.Instance != null)
            {
                InGamePersonaDatasetManager.Instance.OnGameEnd();
            }
        }

        private void GameWin()
        {
            GameEnd();
        }

        private void GameLost()
        {
            GameEnd();
        }
    }
}
