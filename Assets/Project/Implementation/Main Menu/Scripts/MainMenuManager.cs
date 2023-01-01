using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrivingSimulation
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private UI_Level _levelPrefab;
        [SerializeField]
        private Transform _levelContainer;
        
        [Header("Events")]
        [SerializeField]
        private GameEventInt _gotoLevelCallback = null;
        [SerializeField]
        private GameEventObject _onLoadAllLevel = null;

        private void Awake()
        {
            _onLoadAllLevel.AddListener(LoadAllLevel);
        }

        private void OnDestroy()
        {
            _onLoadAllLevel.RemoveListener(LoadAllLevel);
        }

        private void LoadAllLevel(object levelData)
        {
            LevelData ld = levelData as LevelData;

            for (int i = 0; i < ld._levelScenes.Count; i++)
            {
                // spawn level data UI
                UI_Level uiSpawned = Instantiate(_levelPrefab, _levelContainer);
                uiSpawned.gameObject.name = $"{i + 1}: Level Btn";
                uiSpawned.SetText((i+1).ToString());

                int index = i;
                uiSpawned.LevelButton.onClick.AddListener(() => _gotoLevelCallback.Invoke(index));
            }
        }
    }
}
