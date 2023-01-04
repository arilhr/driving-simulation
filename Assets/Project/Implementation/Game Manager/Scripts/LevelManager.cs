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

        [Header("Data")]
        [SerializeField]
        private LevelData _levelDatas;
        [SerializeField]
        private int currentLevel = 0;

        [Header("Events")]
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
        }

        private void Start()
        {
            _onLoadAllLevelCallback.Invoke(_levelDatas);
        }

        private void OnDestroy()
        {
            if (_nextLevelCallback) _nextLevelCallback.RemoveListener(NextLevel);
            if (_gotoLevelCallback) _gotoLevelCallback.RemoveListener(GotoLevel);
        }

        private void NextLevel()
        {
            if (_levelDatas == null)
            {
                Debug.Log("Level Datas is null!");
                return;
            }

            if (currentLevel >= _levelDatas._levelScenes.Count - 1)
            {
                Debug.Log($"Index is greater than total level list!");
                return;
            }

            currentLevel += 1;

            SceneManager.LoadScene(_levelDatas._levelScenes[currentLevel].scene);
        }

        private void GotoLevel(int indexLevel)
        {
            if (_levelDatas == null)
            {
                Debug.Log("Level Datas is null!");
                return;
            }

            if (indexLevel >= _levelDatas._levelScenes.Count || indexLevel < 0)
            {
                Debug.Log($"{indexLevel} Index is greater than total level list or lower than 0!");
                return;
            }

            currentLevel = indexLevel;

            SceneManager.LoadScene(_levelDatas._levelScenes[currentLevel].scene);
        }
    }
}
