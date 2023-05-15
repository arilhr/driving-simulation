using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrivingSimulation
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;



        [Header("Level Training Data")]
        [SerializeField]
        private LevelData _levelDatas;
        [SerializeField]
        private int currentLevel = 0;

        [Header("Level Generation")]
        [Scene]
        public string levelGenerationScene;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _gotoCurrentLevel = null;
        [SerializeField]
        private GameEventNoParam _nextLevelCallback = null;
        [SerializeField]
        private GameEventInt _gotoLevelCallback = null;
        [SerializeField]
        private GameEventObject _onLoadAllLevelCallback = null;

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            _nextLevelCallback.AddListener(NextLevel);
            _gotoLevelCallback.AddListener(GotoLevel);
            _gotoCurrentLevel.AddListener(GotoCurrentLevel);
        }

        private void Start()
        {
            _onLoadAllLevelCallback.Invoke(_levelDatas);
        }

        private void OnDestroy()
        {
            if (_nextLevelCallback) _nextLevelCallback.RemoveListener(NextLevel);
            if (_gotoLevelCallback) _gotoLevelCallback.RemoveListener(GotoLevel);
            if (_gotoCurrentLevel) _gotoCurrentLevel.RemoveListener(GotoCurrentLevel);
        }

        private void GotoCurrentLevel()
        {
            if (_levelDatas == null)
            {
                Debug.Log("Level Datas is null!");
                return;
            }

            if (_levelDatas._levelScenes.Count - 1 < currentLevel)
            {
                Debug.Log($"You already completed all training level! Go to a random challenge generation level.");

                SceneManager.LoadScene(levelGenerationScene);
                return;
            }
        }

        private void NextLevel()
        {
            if (_levelDatas == null)
            {
                Debug.Log("Level Datas is null!");
                return;
            }

            currentLevel += 1;

            GotoCurrentLevel();
        }

        private void GotoLevel(int indexLevel)
        {
            if (_levelDatas == null)
            {
                Debug.Log("Level Datas is null!");
                return;
            }

            currentLevel = indexLevel;

            GotoCurrentLevel();
        }
    }
}
