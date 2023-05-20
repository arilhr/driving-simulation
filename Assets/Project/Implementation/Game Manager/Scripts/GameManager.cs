using SOGameEvents;
using UnityEngine;

namespace DrivingSimulation
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Properties")]
        public bool initOnStart = false;

        [Header("UI")]
        public GameObject gameUI;

        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _initializeGame = null;
        [SerializeField]
        private GameEventNoParam _gameWinCallback = null;
        [SerializeField]
        private GameEventNoParam _gameLostCallback = null;
        [SerializeField]
        private GameEventNoParam _gameEndCallback = null;

        [Header("Input Events")]
        [SerializeField]
        private GameEventBool _setInputActiveCallback = null;

        protected override void Awake()
        {
            base.Awake();

            _initializeGame.AddListener(Initialize);
            _gameWinCallback.AddListener(GameWin);
            _gameLostCallback.AddListener(GameLost);
        }

        private void Start()
        {
            if (initOnStart)
                _initializeGame.Invoke();
        }

        private void OnDestroy()
        {
            _initializeGame.RemoveListener(Initialize);
            _gameWinCallback.RemoveListener(GameWin);
            _gameLostCallback.RemoveListener(GameLost);
        }

        private void Initialize()
        {
            gameUI.SetActive(true);

            Debug.Log("Initialize");
        }

        private void GameEnd()
        {
            _setInputActiveCallback.Invoke(false);

            _gameEndCallback?.Invoke();
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
