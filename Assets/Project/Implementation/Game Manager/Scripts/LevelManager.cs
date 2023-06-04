using SOGameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrivingSimulation
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [Header("Level Training Data")]
        public LevelData levelTrainingData;
        [SerializeField]
        private int currentLevel = 0;

        [Header("Level Generation")]
        [Scene]
        public string automaticLevelGenerateScene;

        [Header("Events")]
        public GameEventNoParam GotoLevelCallback = null;
        public GameEventInt AddLevelCallback = null;

        private const string LEVEL_KEY = "Level";

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

            Load();

            GotoLevelCallback?.AddListener(GotoLevel);
            AddLevelCallback?.AddListener(AddLevel);
        }

        private void OnDestroy()
        {
            GotoLevelCallback?.RemoveListener(GotoLevel);
            AddLevelCallback?.RemoveListener(AddLevel);
        }

        private void GotoLevel()
        {
            if (currentLevel < levelTrainingData.Levels.Count - 1)
            {
                SceneChanger.Instance.LoadScene(levelTrainingData.Levels[currentLevel].scene);
                return;
            }

            SceneChanger.Instance.LoadScene(automaticLevelGenerateScene);
        }

        public void AddLevel(int value)
        {
            currentLevel += value;

            Save();
        }

        private void Load()
        {
            if (ES3.KeyExists(LEVEL_KEY))
                currentLevel = ES3.Load<int>(LEVEL_KEY);
        }

        public void Save()
        {
            ES3.Save(LEVEL_KEY, currentLevel);
        }
    }
}
